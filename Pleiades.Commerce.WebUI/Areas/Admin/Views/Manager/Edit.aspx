<%@ Page Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="ViewPage<UserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.QfRouteButton(QfButtonId.Back, "Back", OutboundNavigation.AdminManagerList()) %> 
    
    <h1>Edit Admin User &gt; <%: Model.Email %></h1>

    <div style="padding-top:30px;">
        <% using (Html.BeginForm("Edit", "Manager", FormMethod.Post, new { enctype = "multipart/form-data" })) 
            { %>
            <div class="dottedline"></div>

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
                <td>
                <% if (this.Model.UserRole != UserRole.Supreme)
                   { %>
                    <%: Html.QfTextEditorFor(x => x.Email, 50)%>
                <% } else { %>
                    <strong><%: Model.Email %></strong>
                    <%: Html.HiddenFor(x => x.Email) %>
                <% } %>
                </td>
            </tr>
            <tr>
                <td class="label-right">Is Approved?:</td>
                <td>
                    <% if (this.Model.UserRole != UserRole.Supreme)
                       { %>
                        <%: Html.QfRadioButtonFor(x => x.IsApproved, "false", "No", style: "float:left;")%>
                    <% } %>
                    
                    <%: Html.QfRadioButtonFor(x => x.IsApproved, "true", "Yes", style: "float:left;")%>
                    <div class="clr" />
                    <%: Html.ValidationMessageFor(x => x.IsApproved) %>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="left">
                    <%: Html.QfRouteButton(
                        QfButtonId.Cancel, 
                        "Details", 
                        OutboundNavigation.AdminManagerDetails(this.RouteData.Values["id"]),
                        style: "float:left;")%>
                    <%: Html.QfSubmitButton(QfButtonId.Save, "save", style: "float:left;") %>
                </td>
            </tr>
            </table>
        <% } %>
    </div>
</asp:Content>