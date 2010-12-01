<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DotNetSqlGenerator.Library.DbProviders.PostgreSQL.PgSqlGenerator>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= ViewData["query"] %></h2>
    <h3>affected rows: <%= ViewData["output"] %></h3>
    <h4>resulting table:</h4>
    <%= ViewData["table"] %>
    


</asp:Content>
