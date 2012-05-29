<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PubSubServiceClient.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            font-size: x-large;
        }
    </style>
</head>
<body bgcolor="#99ff99">
    <form id="form1" runat="server">
    <div>
    
        <span class="style1"><strong>PubSub Service Bus Demo</strong></span><br />
        <br />
    
        <br />
        User:&nbsp;
        <asp:TextBox ID="txtUser" runat="server">guest</asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Priority:&nbsp;
        <asp:DropDownList ID="ddlPriority" runat="server">
            <asp:ListItem Value="0">Low</asp:ListItem>
            <asp:ListItem Value="2">Medium</asp:ListItem>
            <asp:ListItem Value="1">High</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
    
        Message:&nbsp;
        <asp:TextBox ID="txtMessage" runat="server" Height="90px" Width="255px" 
            Font-Size="Medium" TextMode="MultiLine">Hello, Jim!</asp:TextBox>
        <br />
        <br />
    
    </div>
    <asp:Button ID="Button1" runat="server" onclick="btnSend_Click" Text="Send" />
    </form>
</body>
</html>
