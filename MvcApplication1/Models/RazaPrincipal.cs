using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Raza.Model;

namespace MvcApplication1.Models
{

    interface IRazaPrincipal : IPrincipal
    {

        string FirstName { get; }
        string LastName { get; }
        string MemberId { get; set; }

        bool IsEmailSubscribed { get; set; }

        string Pin { get; set; }

        string Email { get; set; }

        string UserType { get; set; }

        string FullName { get; }

    }


    public class RazaPrincipal : IRazaPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public RazaPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email);
            ProfileInfo = new BillingInfo();
        }

        public int UserId { get; set; }

        public string MemberId { get; set; }

        public bool IsEmailSubscribed { get; set; }

        public string Pin { get; set; }

        public string Email { get; set; }

        public string UserType { get; set; }

        public string FullName
        {
            get
            {
              //  return string.Format("{0} {1}", FirstName, LastName);
                string name = string.Empty;
                if (!string.IsNullOrEmpty(FirstName))
                {
                    name = char.ToUpper(FirstName[0]) + FirstName.Substring(1);
                }
                if (!string.IsNullOrEmpty(LastName))
                {
                    name = name + " "+ char.ToUpper(LastName[0]) + LastName.Substring(1);
                       
                }
                return name;
                //return char.ToUpper(FirstName[0]) + FirstName.Substring(1) + char.ToUpper(LastName[0]) +
                //       LastName.Substring(1);

                //char.ToUpper(s[0]) + s.Substring(1);
            }
        }

        public string FirstName
        {
            get
            {
                return ProfileInfo != null ? ProfileInfo.FirstName : string.Empty;
            }
        }

        public string LastName
        {
            get
            {
                return ProfileInfo != null ? ProfileInfo.LastName : string.Empty;
            }
        }

        public BillingInfo ProfileInfo { get; set; }

    }

    public class CustomPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string MemberId { get; set; }

        public bool IsEmailSubscribed { get; set; }

        public string Pin { get; set; }

        public string Email { get; set; }

        public string UserType { get; set; }

        public BillingInfo ProfileInfo { get; set; }

    }

    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new RazaPrincipal User
        {
            get { return base.User as RazaPrincipal; }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new RazaPrincipal User
        {
            get { return base.User as RazaPrincipal; }
        }
    }
}