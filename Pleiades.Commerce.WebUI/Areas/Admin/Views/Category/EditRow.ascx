<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Category>" %>

<tr style="background-color:#999;" class="edittablerow">
    <td style="width:200px; padding-left:5px;">
        <%: Html.InlineGridTextBoxFor(x => x.Name, 20, style: "width: 185px;") %>
    </td>
    <td style="width:400px">
        <%: Html.InlineGridTextBoxFor(x => x.Description, 50, style: "width: 390px;")%>
    </td>
    <td align="center" style="width:75px">
        <%: Html.InlineDropDownlist(x => x.Active, CategorySelectList.Make, style: "width:60px;") %>
    </td>
    <td colspan="3" align="right" style="width:225px">
        <a href="#" style="float:right;" class="savebutton"></a>
        <a href="#" style="float:right;" class="cancelbutton"></a>
        <%: Html.HiddenFor(x => x.Id) %>
    </td>
</tr>
