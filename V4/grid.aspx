<%@ Page Language="VB" AutoEventWireup="false" CodeFile="grid.aspx.vb" Inherits="grid" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
    <link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css" />

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
    <script>
        function redirect(tar, page) {
            $('#frmx').attr("target", tar);
            $('#frmx').attr("action", page);
            // alert("grid.aspx?task=edit&sql" + val + "&tbl=emp_ot");
            $('#frmx').submit();

          
        }
    </script>
    <style type="text/css">
        #viewg input
        {
        	font-size:9pt;
        	height:25px;
        	width:auto;
        	
        	}
        	#viewg 
        {
        	font-size:9pt;
        	
        	
        	
        	}
    </style>
</head>
<body>
  <% Dim sec As New k_security
        Dim sql As String = ""
        Dim tbl As String = ""
        sql = sec.dbHexToStr(Request.QueryString("sql"))
        tbl = Request.QueryString("tbl")
      For Each k As String In Request.Form
          ' Response.Write(k & "==" & Request.form(k) & "<br>")
      Next
      If Request.QueryString("page") <> "" Then
          Session("tar_page") = Request.QueryString("page")
      End If
      ' calledshow(tbl, "", sql)
%>
<asp:Literal ID='ffname' runat="server"></asp:Literal>
<form id='frmx'></form>
     <div id='viewg' style="font-size:8pt;">
      <form id="form1" runat="server">
    <div style="font-size:7pt;">
         <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ></asp:SqlDataSource>    </div>
    </form></div> 
  
</body>
</html>
