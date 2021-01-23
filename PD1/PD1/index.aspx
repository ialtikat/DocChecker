<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="PD1.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//TR" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 423px;
        }
        .auto-style2 {
            margin-left: 214px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">  
    <div style="height: 700px;">  
        <table cellpadding="10" cellspacing="10" width="85%" align="center" style="background: SkyBlue;">  
            <tr>  
                <td class="auto-style1">  
                    &nbsp;<asp:FileUpload ID="WordFileToRead" runat="server" Width="500px" BackColor="#FFFF99" Font-Names="Bahnschrift" Height="26px" />  
                    <asp:Button ID="btnUpload" runat="server" Text="Dosyayı Tara" OnClick="btnUpload_Click" BackColor="#FFFF99" BorderColor="#FFFF99" Font-Names="Bahnschrift" Height="30px" />  
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button1" runat="server" CssClass="auto-style2" OnClick="Button1_Click" Text="Rapor İndir" Width="118px" BackColor="#FFFF99" BorderColor="#FFFF99" Font-Names="Bahnschrift" Height="31px" />
                    </br>
                    <br />
                    <asp:ListBox ID="ListBox1" runat="server" Height="576px" Width="1250px" BackColor="#FFFF99" Font-Names="Bahnschrift"></asp:ListBox>
                    <br />
                    <br />
                </td>  
            </tr>   
        </table>  
    </div>  
    </form>  
</body>
</html>
