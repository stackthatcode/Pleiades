<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Category>" %>

<!-- TODO: inject this into the Table -->
<tr class="displaytablerow">
    <td><%: Model.Name %></td>
    <td><%: Model.Description.TruncateAfter(45)%></td>
    <td align="center"><%: Model.Active%></td>
    <td align="right"><%: Model.LastModified%></td>
    <td align="right">
        <a href="#" class="edit">Edit</a>
        <%: Html.Hidden("Id", Model.Id.ToString())%>
    </td>
</tr>
