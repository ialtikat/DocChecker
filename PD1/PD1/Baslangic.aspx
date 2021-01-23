<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Baslangic.aspx.cs" Inherits="PD1.Baslangic" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="StyleSheet.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
            text-align:left;
            position:absolute;
            top:250px;
            left:0px;
        }
        .auto-style2 {
            height: 22px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ana">
    <div class="ic">

        <asp:Label ID="Label2" CssClass="label" runat="server" Font-Size="Large" ForeColor="White" Text="Lütfen Aşağıdaki Bilgileri Doldurunuz"></asp:Label>
        <asp:Button ID="Button1" CssClass="button" runat="server" BackColor="White" BorderStyle="Double" Font-Bold="True" Text="Giriş" Width="90px" OnClick="Button1_Click1" />
        <br />
        <br />
        <table class="auto-style1">
            <tr>
                <td class="auto-style2">
                    <asp:Label ID="Label3" runat="server" Font-Size="Large" ForeColor="White" Text="Kullanıcı Adı"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="k_adi" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Font-Size="Large" ForeColor="White" Text="Parola"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="sifre" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="yeni">
        <asp:Label ID="Label5" runat="server" Font-Size="Large" ForeColor="White" Text="Üye değilseniz kayıt olmak için"></asp:Label>
&nbsp;<asp:Label ID="Label6" runat="server" Font-Size="Large" ForeColor="White" BackColor="White"><a href="yeni.aspx">tıklayınız</a></asp:Label>
            <br />
            <br />
         <asp:Label ID="Label7" runat="server" ForeColor="White"></asp:Label> 
        </div>
        <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Italic="True" Font-Names="Snap ITC" Font-Size="XX-Large" Font-Strikeout="False" Font-Underline="True" ForeColor="White" Text="HOŞ GELDİNİZ..."></asp:Label>
    </div>
      <div class="baslik">

          <asp:Label ID="Label1" runat="server" Text="TEZ Kontrol Giriş Sayfası" Font-Size="XX-Large"></asp:Label>

      </div>
    </div>
    </form>
</body>
</html>
