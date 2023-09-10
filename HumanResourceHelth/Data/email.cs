using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Data
{
    public class email
    {
        private string _emailHeader;
        private string _emailBody;
        private string _emailFooter;
        //constructor
        public email()
        {
            _emailHeader = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='viewport' content='width=device-width, initial-scale=1.0'><title>Document</title></head><body>";
            _emailBody = "<div style='display:block;text-align:center;min-height: 206px;background-color: #f7f7f7!important;'><img alt='' src='https://www.hrfactoryapp.com/assets/img/logo-1.png' width='196' style='max-width:196px;padding-bottom:0;display:inline!important;vertical-align:bottom;border:0;height:auto;outline:none;text-decoration:none;margin-top:51px' /></div><div style='display: block; background-color: #0c425a; border-top: 0; border-bottom: 0; padding-top: 45px; padding-bottom: 45px; text-align: center '><div style='display:block; margin: 0 -10px;text-align:center; color:#fff'><a href = 'https://twitter.com/HRFactory4' target = '_blank' ><img src = 'https://ci4.googleusercontent.com/proxy/GvgjS4VPlhMl8idO5upbHzEV4AqTNut4mbrm7tN9t-0Y_Os_vvAtMqPaBL6LxSdD50M0_WvdYOaRkeRE25HbR815TslhhzsjoZMzzpKYLiG8MFqu6VDbzkb2JbyH4IjCPWiYy3cT=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-twitter-48.png' alt = 'Twitter' height = '24' width = '24' style = 'text-decoration:none;' /></a ><a href = 'https://www.instagram.com/hrfactoryapp/' target = '_blank' style = 'margin: 20px;' ><img src = 'https://ci5.googleusercontent.com/proxy/Ihh9hEwk_36d3lzL_tLmGaqmGhc-dLqZP-II9LpKgUDCh37Kvw1N4-DJsrxuyAA9V1NNx3975BQO5w7DNVWvFHpPM4gkDm8eMVCLYy_PtGWEZAxMuaULgOR-6W0K_1sgXOcwNMtgGVE=s0-d-e1-ft#https://cdn-images.mailchimp.com/icons/social-block-v2/outline-light-instagram-48.png' alt = 'Link' style = 'text-decoration:none;' height = '24' width = '24' /></a ></div ><hr style = 'min-width:100%;border-top:2px solid #505050;border-collapse:collapse' /><div style = 'padding-top: 0; padding-right: 18px; padding-bottom: 9px; padding-left: 18px; word-break: break-word; color: #ffffff; font-family: Helvetica; font-size: 12px; line-height: 150%; text-align: center; background-color: #0c425a ' ><em > Copyright © 2023 HR Factory, All rights reserved.</em ><br > You are receiving this email because you opted in via our website.<br ><br ><strong > Our mailing address is:</strong ><br ><div ><span > HR Factory </span ><div ><div > MUSCAT,AL - SEEB STREET99 </div ><span > AL - SEEB </span ><span > 121 </span ><div > Oman </div ></div ></div ></div ></div ></body ></html > ";
        }
        //properties
        public string EmailHeader
        {
            get { return _emailHeader; }
            set { _emailHeader = value; }
        }
        public string EmailBody
        {
            get { return _emailBody; }
            set { _emailBody = value; }
        }
        public string EmailFooter
        {
            get { return _emailFooter; }
            set { _emailFooter = value; }
        }

    }
}