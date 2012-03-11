<%@ Page Language="C#" 
    MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<UserViewModel>" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.StandardActionButton(StandardButton.Back, "Details", routeValues: new { id = this.RouteData.Values["id"].ToString() })%>    
    <h1 class="error">Are You Sure You Want to Delete &gt; <%: Model.Email  %> ?</h1>

    <div style="padding-top:30px;">
        <div style="border-top:1px dotted #999; height:20px; width:600px;"></div>

        <%: Html.Partial("AdminUserDetails", this.Model) %>

        <div style="font-size:1px;"></div>
        
         <% using (Html.BeginForm(
                    "DeleteConfirm", "AdminManager", new { id = this.RouteData.Values["id"] }, 
                    FormMethod.Post, new { enctype = "multipart/form-data" }))
            { %>
        <table class="recordgrid" cellpadding="0" cellspacing="0">
        <tr>
            <td class="label-right" style="width:160px; font-weight:bold;">Confirm Delete</td>
            <td><%: Html.StandardActionButton(StandardButton.No, "List", style: "float:left;")%>
                <%: Html.StandardSubmitButton(StandardButton.Yes, "Submit", style: "float:left;")%>
            </td>
        </tr>
        </table>
        <% } %>
    </div>
</asp:Content>
