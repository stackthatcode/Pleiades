<%@ Page Title="Pleiades" Language="C#"
    MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<CreateAdminModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.StandardActionButton(StandardButton.Back, "List") %>
    <h1>Create Admin User</h1>

    <div style="padding-top:30px;">
        <% using (Html.BeginForm("Create", "AdminManager", FormMethod.Post, new { enctype = "multipart/form-data", autocomplete = "off" })) 
            { %>
            <div class="dottedline"></div>
            
            <div style="padding-left:20px; padding-bottom:20px;">
            <%: Html.TagIfNotEmpty(this.ViewData[ViewDataKeys.ErrorMessage], "span", @class: "validation-summary-errors") %>
            </div>

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
                <td><%: Html.StandardTextBoxFor(x => x.Email, 50) %></td>
            </tr>

            <tr>
                <td class="label-right">Password:</td>
                <td><%: Html.StandardPasswordFor(x => x.Password, 25)%></td>
            </tr>
            <tr>
                <td class="label-right">Verify Password:</td>
                <td><%: Html.StandardPasswordFor(x => x.PasswordVerify, 25)%></td>
            </tr>

            <tr>
                <td class="label-right">Is Approved?:</td>
                <td>
                    <%: Html.StandardRadioButton(x => x.IsApproved, "false", "No", style: "float:left;")%>
                    <%: Html.StandardRadioButton(x => x.IsApproved, "true", "Yes", style: "float:left;")%>
                    <div class="clr"></div>
                    <%: Html.ValidationMessageFor(x => x.IsApproved) %>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="left">
                    <div style="width:600px;">
                    <%: Html.StandardActionButton(StandardButton.Cancel, "List", style: "float:left;") %>
                    <%: Html.StandardSubmitButton(StandardButton.Save, "save", style: "float:left;") %>
                    </div>
                </td>
            </tr>
            </table>
        <% } %>
    </div>
</asp:Content>
