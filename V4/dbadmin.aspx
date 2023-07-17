<%@ Page Language="VB" ValidateRequest="false" Debug="true"  CodeFile="dbadmin.aspx.vb"  Inherits="_dbadmin" %>
<%@ Import Namespace="system.IO" %>
<%@ Import Namespace="system.data" %>
<%@ Import Namespace="system.data.sqlclient" %>
<%@ Import Namespace="Kirsoft.hrm" %>
<% 
    Try
        Session("con").open()
    Catch ex As Exception
        Session("con").close()
        Session("con").open()
    End Try
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta content="utf-8" />
	<link rel="stylesheet" href="jqq/themes/base/jquery.ui.all.css">

<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script src="jqq/jquery-1.9.1.js"></script>
	<script src="jqq/ui/jquery.ui.core.js"></script>
	<script src="jqq/ui/jquery.ui.widget.js"></script>
	<script src="jqq/ui/jquery.ui.position.js"></script>
	<script src="jqq/ui/jquery.ui.menu.js"></script>
	<script src="jqq/ui/jquery.ui.autocomplete.js"></script>
	<script src="jqq/ui/jquery.ui.progressbar.js"></script>
	<script src="jqq/ui/jquery.ui.datepicker.js"></script>
	<link rel="stylesheet" href="jq/demos.css" />
	

    <title></title>
</head>
<body>
<% Dim fm As New formMaker
    'Response.Write(fm.helpback(False, True, False, False, "", ""))%>

<%  
    If Session("username") = "" Then
        Response.Redirect("logout.aspx")
    End If
    
    Dim db As New dbclass
    Dim st() As String
    st = db.listtable(session("con"))
 
    For i As Integer = 0 To st.Length - 1
       
         If i = 0 Then
            Response.Write("<table cellspacing='3' style='height:24px; font-size:12pt;border:solid 1px blue; color:white; background:#00554f;width:100px;'><tr style='height:24px;font-size:12pt;border:solid 1px blue; color:white; background:#fdfdfd;width:100px;'>")
        End If
        If i Mod 6 = 0 Then
            Response.Write("</tr><tr style='height:24px;font-size:12pt;border:solid 1px blue; color:white; background:#fdfdfd;width:100px;'>")
        End If
        Response.Write("<td><a href='?dtable=" & st(i) & "' style='text-decoration:none;'>" & st(i) & "</a></td> ")
    Next
    Response.Write("</table><div>")
    %>
    
    <form id="form1" runat="server">
    <div>
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
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ></asp:SqlDataSource>
    </div>
    </form>
    <% Response.Write(pageaddnew()) %>
    <div>
    <%
        If Request.QueryString("dtable") <> "" Then
            ' Response.Write(dbxmlview(Request.QueryString("dtable")))
        End If%>
        </div>
   <% 'Session("con").close()%>
</body>
</html>
