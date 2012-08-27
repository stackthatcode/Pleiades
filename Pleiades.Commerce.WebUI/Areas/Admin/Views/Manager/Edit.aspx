<%@ Page Title="Pleiades" 
    Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<UserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.StandardActionButton(StandardButton.Back, 
            "Details", routeValues: new { id = this.RouteData.Values["id"].ToString() })%>
    
    <h1>Edit Admin User &gt; <%: Model.Email %></h1>

    <div style="padding-top:30px;">
        <% using (Html.BeginForm("Edit", "AdminManager", FormMethod.Post, new { enctype = "multipart/form-data" })) 
            { %>
            <div class="dottedline"></div>

            <table class="recordgrid" cellpadding="0" cellspacing="0">
            <tr>
                <td class="label-right" style="width:170px;">First Name:</td>
                <td><%: Html.StandardTextBoxFor(x =>  x.FirstName, 25) %></td>
            </tr>
            <tr>
                <td class="label-right">Last Name:</td>
                <td <%: Html.StandardTextBoxFor(x =>  x.LastName, 25) %></td>
            </tr>
            <tr>
                <td class="label-right">Email:</td>
                <td>
                <% if (this.Model.UserRole != UserRole.Root)
                   { %>
                    <%: Html.StandardTextBoxFor(x => x.Email, 50)%>
                <% } else { %>
                    <strong><%: Model.Email %></strong>
                <% } %>
                </td>
            </tr>
            <tr>
                <td class="label-right">Is Approved?:</td>
                <td>
                    <% if (this.Model.UserRole != UserRole.Root)
                       { %>
                        <%: Html.StandardRadioButton(x => x.IsApproved, "false", "No", style: "float:left;")%>
                    <% } %>
                    
                    <%: Html.StandardRadioButton(x => x.IsApproved, "true", "Yes", style: "float:left;")%>
                    <div class="clr" />
                    <%: Html.ValidationMessageFor(x => x.IsApproved) %>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="left">
                    <%: Html.StandardActionButton(StandardButton.Cancel, 
                            "Details", style: "float:left;", 
                                routeValues: new { id = this.RouteData.Values["id"].ToString() })%>
                    <%: Html.StandardSubmitButton(StandardButton.Save, "save", style: "float:left;") %>
                </td>
            </tr>
            </table>
        <% } %>
    </div>
</asp:Content>
