<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IPagedList<Category>>" %>


<div style="position:relative;">
    <div style="top:-40px; left:600px; position:absolute; width:400px; text-align:right;">
        <a href="#" style="float:right;" class="addnewbutton"></a>
        <h4 style="color:#999; float:right; padding-top:5px; padding-right:10px; z-index:10;">Click here to add a new Category: </h4>
    </div> 
</div>

<table cellpadding="0" cellspacing="0" class="datagrid" id="grid" style="width:1000px;">
    <thead>
        <tr>
            <td style="width:200px"><a href="" class="orderbylink">Name</a></td>
            <td style="width:400px"><a href="" class="orderbylink">Description</a></td>
            <td align="center" style="width:100px"><a href="" class="orderbylink">Active</a></td>
            <td align="center" style="width:150px">Last Modified</td>
            <td align="right" style="width:75px;">Delete</td>
            <td align="right" style="width:75px;">Edit</td>
        </tr>
    </thead>
    <tbody>
    <% foreach (var row in Model)
        { %>
        <tr class="displaytablerow">
            <td><%: row.Name %></td>
            <td><%: row.Description.TruncateAfter(45) %></td>
            <td align="center"><%: row.ActiveYesNo %></td>
            <td align="right"><%: row.LastModified %></td>
            <td align="right">
                <a href="#" class="deletelink">Delete</a>
                <%: Html.Hidden("Id", row.Id.ToString()) %>
            </td>
            <td align="right">
                <a href="#" class="editlink">Edit</a>
                <%: Html.Hidden("Id", row.Id.ToString()) %>
            </td>
        </tr>
    <% } %>
    </tbody>
</table>

<% if (Model.Count == 0)
   { %>
<div id="empty" style="padding-top:95px; color:#999; text-align:center;">
    <h1>There are no Categories in the Database</h1>
</div>
<% } %>
<div style="clear:both; height:20px;"></div>
<%: Html.StandardPageLinks(Model, (Page) => { return "#"; }) %>
