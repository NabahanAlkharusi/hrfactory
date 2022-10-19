using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class UriHelper
    {
        /* IsLocal; when running in web context:
         * [1] give LinkProvider http[s] scheme; see CreateBase(string baseUri)
         * [2] give ImageProvider relative path starting with '/' - see:
         *     Join(string relativeUri)
         */
        public bool IsLocal { get; set; }
        public HttpContext HttpContext { get; private set; }
        public Uri BaseUri { get; private set; }

        public UriHelper(string baseUri) : this(baseUri, true) { }
        public UriHelper(string baseUri, bool isLocal)
        {
            IsLocal = isLocal;
            HttpContext = HttpContext.Current;
            BaseUri = CreateBase(baseUri);
        }

        /* get URI for IImageProvider to instantiate iTextSharp.text.Image for 
         * each <img> element in the HTML.
         */
        public string Combine(string relativeUri)
        {
            /* when running in a web context, the HTML is coming from a MVC view 
             * or web form, so convert the incoming URI to a **local** path
             */
            if (HttpContext != null && !BaseUri.IsAbsoluteUri && IsLocal)
            {
                return HttpContext.Server.MapPath(
                    // Combine() checks directory traversal exploits
                    VirtualPathUtility.Combine(BaseUri.ToString(), relativeUri)
                );
            }
            return BaseUri.Scheme == Uri.UriSchemeFile
                ? Path.Combine(BaseUri.LocalPath, relativeUri)
                // for this example we're assuming URI.Scheme is http[s]
                : new Uri(BaseUri, relativeUri).AbsoluteUri;
        }

        private Uri CreateBase(string baseUri)
        {
            if (HttpContext != null)
            {   // running on a web server; need to update original value  
                var req = HttpContext.Request;
                baseUri = IsLocal
                    // IImageProvider; absolute virtual path (starts with '/')
                    // used to convert to local file system path. see:
                    // Combine(string relativeUri)
                    ? req.ApplicationPath
                    // ILinkProvider; absolute http[s] URI scheme
                    : req.Url.GetLeftPart(UriPartial.Authority)
                        + HttpContext.Request.ApplicationPath;
            }

            Uri uri;
            if (Uri.TryCreate(baseUri, UriKind.RelativeOrAbsolute, out uri)) return uri;

            throw new InvalidOperationException("cannot create a valid BaseUri");
        }
    }
    public class LinkProvider : ILinkProvider
    {
        // rfc1738 - file URI scheme section 3.10
        public const char SEPARATOR = '/';
        public string BaseUrl { get; private set; }

        public LinkProvider(UriHelper uriHelper)
        {
            var uri = uriHelper.BaseUri;
            /* simplified implementation that only takes into account:
             * Uri.UriSchemeFile || Uri.UriSchemeHttp || Uri.UriSchemeHttps
             */
            BaseUrl = uri.Scheme == Uri.UriSchemeFile
                // need trailing separator or file paths break
                ? uri.AbsoluteUri.TrimEnd(SEPARATOR) + SEPARATOR
                // assumes Uri.UriSchemeHttp || Uri.UriSchemeHttps
                : BaseUrl = uri.AbsoluteUri;
        }

        public string GetLinkRoot()
        {
            return BaseUrl;
        }
    }

    public class ImageProvider : IImageProvider
    {
        private UriHelper _uriHelper;
        // see Store(string src, Image img)
        private Dictionary<string, iTextSharp.text.Image> _imageCache =
            new Dictionary<string, iTextSharp.text.Image>();

        public virtual float ScalePercent { get; set; }
        public virtual Regex Base64 { get; set; }

        public ImageProvider(UriHelper uriHelper) : this(uriHelper, 67f) { }
        //              hard-coded based on general past experience ^^^
        // but call the overload to supply your own
        public ImageProvider(UriHelper uriHelper, float scalePercent)
        {
            _uriHelper = uriHelper;
            ScalePercent = scalePercent;
            Base64 = new Regex( // rfc2045, section 6.8 (alphabet/padding)
                @"^data:image/[^;]+;base64,(?<data>[a-z0-9+/]+={0,2})$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase
            );
        }

        public virtual iTextSharp.text.Image ScaleImage(iTextSharp.text.Image img)
        {
            img.ScalePercent(ScalePercent);
            return img;
        }

        public virtual iTextSharp.text.Image Retrieve(string src)
        {
            if (_imageCache.ContainsKey(src)) return _imageCache[src];

            try
            {
                if (Regex.IsMatch(src, "^https?://", RegexOptions.IgnoreCase))
                {
                    return ScaleImage(iTextSharp.text.Image.GetInstance(src));
                }

                Match match;
                if ((match = Base64.Match(src)).Length > 0)
                {
                    return ScaleImage(iTextSharp.text.Image.GetInstance(
                        Convert.FromBase64String(match.Groups["data"].Value)
                    ));
                }

                var imgPath = _uriHelper.Combine(src);
                return ScaleImage(iTextSharp.text.Image.GetInstance(imgPath));
            }
            // not implemented to keep the SO answer (relatively) short
            catch (BadElementException) { return null; }
            catch (IOException) { return null; }
            catch (Exception) { return null; }
        }

        public virtual void Store(string src, iTextSharp.text.Image img)
        {
            if (!_imageCache.ContainsKey(src)) _imageCache.Add(src, img);
        }

        public virtual string GetImageRootPath() { return null; }
        public virtual void Reset() { }

        /*
         * always called after Retrieve(string src):
         * [1] cache any duplicate <img> in the HTML source so the image bytes
         *     are only written to the PDF **once**, which reduces the 
         *     resulting file size.
         * [2] the cache can also **potentially** save network IO if you're
         *     running the parser in a loop, since Image.GetInstance() creates
         *     a WebRequest when an image resides on a remote server. couldn't
         *     find a CachePolicy in the source code
         */


    }
    public class SimpleParserr
    {
        public virtual ILinkProvider LinkProvider { get; set; }
        public virtual IImageProvider ImageProvider { get; set; }

        public virtual HtmlPipelineContext HtmlPipelineContext { get; set; }
        public virtual ITagProcessorFactory TagProcessorFactory { get; set; }
        public virtual ICSSResolver CssResolver { get; set; }

        /* overloads simplfied to keep SO answer (relatively) short. if needed
         * set LinkProvider/ImageProvider after instantiating SimpleParser()
         * to override the defaults (e.g. ImageProvider.ScalePercent)
         */
        public SimpleParserr() : this(null) { }
        public SimpleParserr(string baseUri)
        {
            LinkProvider = new LinkProvider(new UriHelper(baseUri, false));
            ImageProvider = new ImageProvider(new UriHelper(baseUri, true));

            HtmlPipelineContext = new HtmlPipelineContext(null);

            // another story altogether, and not implemented for simplicity 
            TagProcessorFactory = Tags.GetHtmlTagProcessorFactory();
            CssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
        }

        /*
         * when sending XHR via any of the popular JavaScript frameworks,
         * <img> tags are **NOT** always closed, which results in the 
         * infamous iTextSharp.tool.xml.exceptions.RuntimeWorkerException:
         * 'Invalid nested tag a found, expected closing tag img.' a simple
         * workaround.
         */
        public virtual string SimpleAjaxImgFix(string xHtml)
        {
            return Regex.Replace(
                xHtml,
                "(?<image><img[^>]+)(?<=[^/])>",
                new MatchEvaluator(match => match.Groups["image"].Value + " />"),
                RegexOptions.IgnoreCase | RegexOptions.Multiline
            );
        }

        public virtual void Parse(Stream stream, string xHtml, string compName, string docName, int userId, string logoFilePath, string cssPath)
        {
            try
            {
                xHtml = SimpleAjaxImgFix(xHtml);
                docName = "هاند بوك";
                using (var stringReader = new StringReader(xHtml))
                {
                    using (Document document = new Document())
                    {
                        PdfWriter writer = PdfWriter.GetInstance(document, stream);
                        document.Open();
                        document.NewPage();
                        FontFactory.Register("C:\\Windows\\Fonts\\ARIAL.TTF", "arial unicode ms");
                        ///////
                        HtmlPipelineContext
                            .SetTagFactory(Tags.GetHtmlTagProcessorFactory())
                            .SetLinkProvider(LinkProvider)
                            .SetImageProvider(ImageProvider)
                        ;
                        var pdfWriterPipeline = new PdfWriterPipeline(document, writer);
                        var htmlPipeline = new HtmlPipeline(HtmlPipelineContext, pdfWriterPipeline);
                        var cssResolverPipeline = new CssResolverPipeline(CssResolver, htmlPipeline);

                        XMLWorker worker = new XMLWorker(cssResolverPipeline, true);
                        XMLParser parser = new XMLParser(worker);
                        parser.Parse(stringReader);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;

            }
        }

        // write on end of each page
    }
}

public class PageImprove : PdfPageEventHelper

{
    public override void OnStartPage(PdfWriter writer, Document document)
    {
        Chunk ch = new Chunk("Page " + writer.PageNumber);
        document.Add(ch);
    }
}

