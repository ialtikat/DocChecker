<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="yeni.aspx.cs" Inherits="PD1.yeni" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="StyleSheet.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
            position: absolute;
            top: 50px;
            left: 0px;
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="ana">
            <div class="ic2">

                <table class="auto-style1">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" ForeColor="White" Text="Kullanıcı Adı"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="k_adi" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" ForeColor="White" Text="Şifre"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="sifre" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" ForeColor="White" Text="Şifre (Tekrar)"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="sifre_tekrar" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <asp:Button ID="Button1" CssClass="button2" runat="server" Text="Kayıt ol" BorderColor="Maroon" BorderStyle="Outset" OnClick="Button1_Click" />

                <asp:Label ID="Label5" runat="server" Text="Lütfen Aşağıdaki Bilgileri Doldurunuz..." ForeColor="White"></asp:Label>

                <asp:Label ID="Label6" CssClass="aciklama" runat="server" ForeColor="White"></asp:Label>

            </div>
            <div class="baslik">

                <asp:Label ID="Label1" runat="server" Text="TEZ Kontrol Kayıt Sayfasına Hoş Geldiniz..." Font-Size="XX-Large"></asp:Label>

            </div>
        </div>
    </form>
</body>
</html>
