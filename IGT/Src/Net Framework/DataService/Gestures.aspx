<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gestures.aspx.cs" Inherits="DataService.Gestures" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .result
        {
            padding: 10px;
            background: lightyellow;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    Account
                </td>
                <td>
                    Project
                </td>
                <td>
                    Recorded Gesture
                </td>
                <td>Enable Gestures Events</td>
            </tr>
            <tr>
                <td>
                    <asp:ListBox ID="accountListBox" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="accountListBox_SelectedIndexChanged" Rows="10"></asp:ListBox>
                </td>
                <td>
                    <asp:ListBox ID="projectListBox" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="projectListBox_SelectedIndexChanged" Rows="10"></asp:ListBox>
                </td>
                <td>
                    <asp:ListBox ID="gestureListBox" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="gestureListBox_SelectedIndexChanged" Rows="10"></asp:ListBox>
                </td>
                <td>
                
                    <asp:ListBox ID="enabledGesturesListBox" runat="server" Rows="10" 
                        SelectionMode="Multiple">
                        <asp:ListItem Selected="True">lasso</asp:ListItem>
                        <asp:ListItem Selected="True">move</asp:ListItem>
                        <asp:ListItem Selected="True">zoom</asp:ListItem>
                        <asp:ListItem Selected="True">pinch</asp:ListItem>
                        <asp:ListItem>rotate</asp:ListItem>
                        <asp:ListItem Selected="True">tap</asp:ListItem>
                        <asp:ListItem Selected="True">doubletap</asp:ListItem>
                    </asp:ListBox>
                
                </td>
            </tr>
        </table>
    </div>
    <asp:Button ID="generatePageButton" runat="server" Text="Generate Page" 
        onclick="generatePageButton_Click" />

    <div class="result" >
        <asp:HyperLink ID="testPageUrl" runat="server"></asp:HyperLink>
    </div>
    </form>
</body>
</html>
