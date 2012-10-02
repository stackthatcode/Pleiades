<%@ Page Title="Pleiades" Language="C#"
    MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<CreateAdminModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.QfActionButton(QfButtonId.Back, "List") %>
    <h1>Create Admin User</h1>

    <div style="padding-top:30px;">
        <% using (Html.BeginForm("Create", "Manager", FormMethod.Post, new { enctype = "multipart/form-data", autocomplete = "off" })) 
            { %>
            <div class="dottedline"></div>
            
            <div style="padding-left:20px; padding-bottom:20px;">
                <%: Html.QfValidationSummary() %>
            </div>

            <table class="recordgrid" cellpadding="0" cellspacing="0">
            <tr>
                <td class="label-right" style="width:170px;">First Name:</td>
                <td><%: Html.QfTextEditorFor(x => x.FirstName, 25) %></td>
            </tr>
            <tr>
                <td class="label-right">Last Name:</td>
                <td><%: Html.QfTextEditorFor(x => x.LastName, 25) %></td>
            </tr>
            <tr>
                <td class="label-right">Email:</td>
                <td><%: Html.QfTextEditorFor(x => x.Email, 50)%></td>
            </tr>

            <tr>
                <td class="label-right">Password:</td>
                <td><%: Html.QfPasswordFor(x => x.Password, 25)%></td>
            </tr>
            <tr>
                <td class="label-right">Verify Password:</td>
                <td><%: Html.QfPasswordFor(x => x.PasswordVerify, 25)%></td>
            </tr>

            <tr>
                <td class="label-right">Is Approved?:</td>
                <td>
                    <%: Html.QfRadioButtonFor(x => x.IsApproved, "false", "No", style: "float:left;")%>
                    <%: Html.QfRadioButtonFor(x => x.IsApproved, "true", "Yes", style: "float:left;")%>
                    <div class="clr"></div>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="left">
                    <div style="width:600px;">
                    <%: Html.QfActionButton(QfButtonId.Cancel, "List", style: "float:left;") %>
                    <%: Html.QfSubmitButton(QfButtonId.Save, "save", style: "float:left;") %>
                    </div>
                </td>
            </tr>
            </table>
        <% } %>
    </div>
</asp:Content>
