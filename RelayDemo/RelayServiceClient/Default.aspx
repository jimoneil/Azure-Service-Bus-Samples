<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RelayServiceClient.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .rbstyle
        {
            margin-left: 20px;
        }
    </style>
</head>
<body bgcolor="#ffffcc">
    <form id="form1" runat="server">
    <div>
    
        <span class="style1"><strong style="font-size: x-large">WCF/Service Bus Relay Client</strong></span><br />
        <br />
        <br />
        User:&nbsp;
        <asp:TextBox ID="txtUser" runat="server">guest</asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Color:&nbsp;
        <asp:DropDownList ID="ddlColors" runat="server">
            <asp:ListItem>SkyBlue</asp:ListItem>
            <asp:ListItem>Pink</asp:ListItem>
            <asp:ListItem>Plum</asp:ListItem>
            <asp:ListItem>BurlyWood</asp:ListItem>
            <asp:ListItem>Tomato</asp:ListItem>            
            <asp:ListItem>PaleGreen</asp:ListItem>                    
        </asp:DropDownList>
        <br />
        <br />
    
        Message:&nbsp;
        <asp:TextBox ID="txtMessage" runat="server" Height="90px" Width="275px" 
            Font-Size="Medium" TextMode="MultiLine">Hello World!</asp:TextBox>
        <br />
        <br />
        Invoke:<asp:RadioButtonList 
            ID="rbServiceType" runat="server" 
            CellPadding="0" CellSpacing="10" CssClass="rbstyle" 
            style="margin-bottom: 0px">
            <asp:ListItem Selected="True" Value="0">Via WCF Service</asp:ListItem>
            <asp:ListItem Value="1">Via ServiceBus Relay</asp:ListItem>
        </asp:RadioButtonList>
        <br />
    
    </div>
    <asp:Button ID="Button1" runat="server" onclick="btnSend_Click" 
        Text="Send Message" />
    </form>
</body>
</html>
