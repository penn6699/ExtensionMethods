<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ExtensionMethodsDemo.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Index demo</title>
    <style>
        div {
            margin:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <%
            string dateStr = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            %>
        <div>dateStr: <%=dateStr %></div>
        
    </form>
</body>
</html>
