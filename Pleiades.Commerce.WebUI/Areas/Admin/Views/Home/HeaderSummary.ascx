<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Pleiades.Web.Security.Model.DomainUser>" %>

<span style="float:right; color:#FFF; padding-top:20px; text-align:right;">
Logged on as: <%: Model.MembershipUser.Email %>
</span>
