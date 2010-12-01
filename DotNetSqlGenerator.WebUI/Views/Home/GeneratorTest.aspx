<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DotNetSqlGenerator.Library.DbProviders.PostgreSQL.PgSqlGenerator>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	GeneratorTest
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>GeneratorTest</h2>
    <h3>Table Names</h3>
    <% foreach (string s in Model.TableNames) { %>
            <%= s %><br />
    <% }  %>

</asp:Content>
