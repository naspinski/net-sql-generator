<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DotNetSqlGenerator.Library.DbProviders.PostgreSQL.PgSqlGenerator>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: ViewData["Message"] %></h2>

    
    <h2>PgSqlGeneratorTest</h2>
    <h3>Table Names</h3>
    <% foreach (string s in Model.TableNames) { %>
            <%= s %><br />
    <% }  %>
</asp:Content>
