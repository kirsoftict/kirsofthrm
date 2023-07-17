<%@ Page Language='VB' AutoEventWireup='false' CodeFile='perdiemsheet.aspx.vb' Inherits='perdiemsheet' %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>

<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>

<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
<title>Untitled Document</title>
<script type="text/javascript" src="scripts/kirsoft.java.js"></script>
<script src="jqq/jquery-1.9.1.js"></script>
<script>
    $(document).ready(function () {
        parent.calledsee();
    });
	</script>
<script language="javascript" type="text/javascript">
function print(loc,title,head,footer)
    { 
  
    var printFriendly = document.getElementById(loc);
    var printcode=document.getElementById('code1');
    var printstyle=document.getElementById('style1');
    var printWin = window.open("printview.htm",title,"menubar=no;status=no;toolbar=no;");
    printWin.document.write('<html><head><title>' + head +'</title>' + printcode.innerHTML +"<style>"+ printstyle.innerHTML + '</style></head><body>' + printFriendly.innerHTML + "<label class='smalllbl'>"+footer + "</label></body></html>");
    printWin.document.close();
    printWin.window.print();   
   // printWin.close();
    }


</script>
<style type='text/css' id='style1'>
    .headerp {
 text-align: center;
 font-weight: bold;
}
.tcenter {
 text-align: right;
 font-weight: bold;
}
#tblneed 
{
 border: 1px solid #000;
 }
 #tblneed td
 {
 border-right: 1px solid #000;
	border-bottom: 1px solid #000;
  }
</style> 
<script type="text/javascript" src="scripts/kirsoft.java.js"></script>
</head>

<body style="font-size: 12pt">

<div id='result'>Saving...</div>

<%  Dim fm As New formMaker
        Dim sec As New k_security
        Dim ds As New dbclass
        Dim emptid As String
    Dim arrp() As String
    Dim t1, t2 As Date
    Dim rtn As String
    t1 = Now
    Dim spl(1) As String
    Dim i As Integer = 0
    If Request.QueryString("save") = "on" Then
        'Response.Write(Request.QueryString("paiddate"))
        If Request.QueryString("paiddate") <> "" Then
        
            Dim pdate As Date = Request.QueryString("paiddate")
            Dim pmthd As String = Request.QueryString("mthd")
            For Each m As String In Request.QueryString
            
                spl = Request.QueryString(m).Split("-")
                If spl.Length > 1 Then
                    ReDim Preserve arrp(i + 1)
                    emptid = spl(0)
                    arrp(i) = spl(1)
                    i = i + 1
                End If
            Next%>
<div id='printview' style="font-size:12pt;">

<table cellpadding='2' cellspacing='0' width='600'>
  
   <tr>
    <td colspan='13' class='headerp'><center><strong><% response.write(session("company_name")) %></strong></center></td>
  </tr>
  <tr>
    <td colspan='13'  class='headerp'><center><strong>Addis Ababa</strong></center></td>
  </tr>
  <tr>
    <td colspan='13'  class='headerp'><center><strong>Perdiem Settlement    Form</strong></center></td>
  </tr>
   <tr>
    <td colspan='13'  class='headerp' id="refid"></td>
  </tr>
  <tr>
    <td style="height: 23px"></td>
      <td colspan="1" style="height: 23px" width="0">
      </td>
      <td colspan="1" style="height: 23px">
      </td>
      <td colspan="1" style="height: 23px">
      </td>
      <td colspan="1" style="height: 23px">
          <strong></strong>
      </td>
      <td colspan="1" style="width: 120px; height: 23px">
          <strong>Date:</strong></td>
      <td colspan="12" style="height: 23px">
        <span style="text-decoration: underline">&nbsp; 
        <% Response.Write(MonthName(pdate.Month) & " " & pdate.Day.ToString & "," & pdate.Year.ToString)%></span>
      </td>
  </tr>
  <tr>
      <td colspan="13" style="height: 23px">
      </td>
  </tr>
  <tr>
      <td colspan="2" style="height: 22px; width: 217px;">
        <strong>Traveller's Name:</strong></td>
    <td colspan='7' style="height: 22px"><% Response.Write(fm.getfullname(fm.getinfo2("select emp_id from emprec where id=" & emptid, Session("con")), Session("con")))%></td>
      <td colspan="1" style="height: 22px">
      </td>
    <td style="height: 22px" colspan="3"></td>
   
  </tr>
  <tr>
      <td colspan="2" style="width: 217px">
        <strong>Position:</strong></td>
    <td colspan='7'><% Response.Write(fm.getinfo2("select position from emp_job_assign where emptid=" & emptid & " and date_end is null", Session("con")))%></td>
      <td colspan="1">
      </td>
    <td colspan="4"></td>
  </tr>
  <tr>
      <td colspan="2" style="width: 217px; height: 16px;">
        <strong>project Name: &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</strong>&nbsp; &nbsp;</td>
    <td colspan='7' style="height: 16px"><% Response.Write(fm.getinfo2("select project_name from tblproject where project_id in(select project_id from emp_job_assign where emptid=" & emptid & " and date_end is null)", Session("con")))%></td>
      <td colspan="1" style="height: 16px">
      </td>
    <td style="height: 16px"></td>
    <td style="width: 9px; height: 16px"></td>
    <td colspan="2" style="height: 16px"></td>
  </tr>
  
  <tr>
      <td colspan="2" style="width: 217px">
        <strong>Perdiem Rate (Birr):</strong></td>
    <td colspan='7'><%  Dim rate As Double = CDbl(fm.numdigit(fm.getinfo2("select pardim from pardimpay where emptid=" & emptid & " order by id desc ", Session("con")), 2))
                        Response.Write(rate.ToString)%></td>
      <td colspan="1">
      </td>
    <td></td>
    <td style="width: 9px"></td>
    <td colspan="2"></td>
  </tr>
  <tr>
    <td style="height: 21px;" colspan="13" rowspan="2">
        &nbsp;</td><td></td>
  </tr>
  <tr>
  </tr>
  <tr>
    <td colspan='12' rowspan='6'>
    <table align='center' id='tblneed' cellpadding='0' cellspacing='0'><tr><td><strong>Purpose for Travel</strong></td><td>
        <strong>Departure Date (D/M/Y)</strong></td>
    <td width='9'>&nbsp;</td>
    <td >
        <strong>Return Date (D/M/Y)</strong></td>
    <td width='77'>
        <strong>No. Days</strong></td>
    
  </tr>
  <%  Dim ref As String
      ref = Now.Ticks
      
      ' ref = sec.StrToHex(ref)
      
      ' Dim sql() As String = {"", "", "", "", "", ""}
      Dim sql() As String
      Dim sumdays As Integer = 0
      Dim noday As Integer
      Dim adv As Double = 0
      Dim paypar As Double = 0
      Dim advtext As String = 0
      Dim j As Integer = 0
      
      For i = 0 To UBound(arrp)
          If String.IsNullOrEmpty(arrp(i)) = False Then
              ReDim Preserve sql(j + 1)
              sql(j) = "Update pardimpay set ref='" & ref & "',paid_date='" & pdate & "',mthd='" & pmthd & "',paid_state='y' where id=" & arrp(i).ToString
              j += 1
              noday = fm.getinfo2("select no_days from pardimpay where id=" & arrp(i), Session("con"))
              %>
  <tr>
  <td width='177'><%  Response.Write(fm.getinfo2("select reason from pardimpay where id=" & arrp(i), Session("con")))%></td>
    <td width='177'><%  Response.Write(CDate(fm.getinfo2("select from_date from pardimpay where id=" & arrp(i), Session("con"))).ToShortDateString)%></td>
    <td>&nbsp;</td>
    <td width='177'><%  Response.Write(CDate(fm.getinfo2("select to_date from pardimpay where id=" & arrp(i), Session("con"))).ToShortDateString)%></td>
    <td><%  Response.Write(fm.getinfo2("select no_days from pardimpay where id=" & arrp(i), Session("con")))%>
    </td>
 
  
  </tr>
  <%  advtext = fm.getinfo2("select adv from pardimpay where id=" & arrp(i), Session("con")).ToString
      Response.Write(advtext)
      If String.IsNullOrEmpty(advtext) = False Then
          If advtext <> "None" Then
              ' Response.Write(advtext)
              adv += CDbl(advtext.ToString)
          End If
      End If
      sumdays += CDbl(noday)
      
  End If
      
Next
%>

 
  <tr>
    <td colspan='4' class='tcenter'>Total No. of Days</td>
    <td>&nbsp;<%  Response.Write(sumdays.ToString)
                  ' Response.Write(ref)
                  %></td>
     </tr>
    </table></td><td></td>
  </tr>
  <tr>
    <td style="width: 26px" rowspan="5"></td>
  </tr>
  <tr>
  </tr>
  <tr>
  </tr>
  <tr>
  </tr>
  <tr>
  </tr>
  <tr>
    <td style="width: 2060px"></td>
    <td colspan="4"></td>
    <td colspan="3"></td>
      <td colspan="1">
      </td>
    <td></td>
    <td></td>
    <td style="width: 9px"></td>
    <td style="width: 26px"></td>
  </tr>
  <tr>
    <td style="width: 2060px; height: 23px"></td>
      <td colspan="5" style="height: 23px">
          Perdiem Amount (Birr):-</td>
    <td style="height: 23px" colspan="5"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <% 
                                              paypar = CDbl(rate) * CDbl(sumdays)
                                              Response.Write(fm.numdigit(paypar, 2).ToString)%> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
    <td style="height: 23px"></td>
    
  </tr>
  <tr>
    <td style="width: 2060px"></td>
    <td colspan="4"></td>
    <td colspan="3"></td>
      <td colspan="1">
      </td>
    <td></td>
    <td></td>
    <td style="width: 9px"></td>
    <td style="width: 26px"></td>
  </tr>
  <tr>
    <td style="width: 217px; height: 23px" colspan="2">
        <strong>Amount in Words:</strong></td>
    <td colspan='11' style="height: 23px"><span id='amtword-<%response.write(ref)%>' style="font-style:italic; text-decoration:underline;"></span><script type="text/javascript">getwordjs('<%response.write(paypar)%>','amtword-<% response.write(ref) %>');</script></td>
    <td style="height: 23px; width: 26px;"></td>
  </tr>
  <tr>
    <td style="height: 23px" colspan="3">
        <strong>perdiem expense</strong></td>
    <td colspan='3' style="height: 23px; text-align:right;"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <% Response.Write(fm.numdigit(paypar, 2).ToString)%>&nbsp;</td>
      <td colspan="6" style="height: 23px">
      </td>
    <td style="height: 23px; width: 26px;"></td>
  </tr>
  <tr>
    <td colspan="3">
        <strong>advance cpv#</strong></td>
    <td colspan='3' style="text-align:right;">(<% Response.Write(fm.numdigit(adv, 2).ToString)%>)</td>
      <td colspan="7">
      </td>
  </tr>
  <tr>
    <td style="height: 20px" colspan="3">
        <strong>Balance Due to Traveller:</strong></td>
    <td colspan='3' style="height: 20px; text-align:right"><%  Response.Write(fm.numdigit(cdbl(paypar)-cdbl(adv), 2).ToString)      
    %></td>
    <td style="height: 20px" colspan="7"></td>
  </tr>
  <tr>
    <td colspan="2" style="width: 217px"></td>
    <td colspan="4"></td>
    <td colspan="7"></td>
  </tr>
  <tr>
    <td style="height: 23px; width: 217px;" colspan="2"></td>
    <td colspan='3' style="height: 23px"><strong>Payment Method</strong></td>
    <td style="height: 23px" colspan="8">&nbsp;<% Response.Write(pmthd)%></td>
  </tr>
  <tr>
    <td style="height: 23px;" colspan="13"></td>
  </tr>
   <tr>
    <td style="height: 23px;" colspan="13"></td>
  </tr>
  <tr>
    <td colspan="13" rowspan="2"></td>
  </tr>
  <tr>
  </tr>
  <tr>
    <td colspan="13" rowspan="2"></td>
  </tr>
  <tr>
  </tr>
  <tr>
    <td colspan="13"></td>
  </tr>
  <tr>
    <td colspan="13"></td>
  </tr>
  <tr>
    <td style="height: 22px;" colspan="13"></td>
  </tr>
  <tr>
    <td style="width: 206px">_______________</td>
    <td colspan='3'>____________</td>
    <td style="width: 184px"></td>
    <td style="width: 120px">____________</td>
    <td style="width: 321px"></td>
    <td>____________</td>
      <td>
      </td>
    <td></td>
    <td>____________</td>
    <td style="width: 9px"></td>
    <td style="width: 26px"></td>
  </tr>
  <tr>
    <td style="width: 206px">Receiver's Signature</td>
    <td colspan='3'>Prepared&nbsp; By</td>
    <td style="width: 184px"></td>
    <td style="width: 120px">Checked By</td>
    <td style="width: 321px"></td>
    <td>Verified By</td>
      <td>
      </td>
    <td></td>
    <td colspan='2'>Approved By</td>
    <td style="width: 26px"></td>
  </tr>
  <tr>
    <td style="width: 200px">____________</td>
    <td colspan='3'>____________</td>
    <td style="width: 184px">&nbsp;</td>
    <td style="width: 120px">____________</td>
    <td style="width: 321px">&nbsp;</td>
    <td>____________</td>
      <td>
      </td>
    <td>&nbsp;</td>
    <td>____________</td>
    <td style="width: 9px">&nbsp;</td>
    <td style="width: 26px"></td>
  </tr>
  <tr>
    <td style="width: 2px">  &nbsp;Date</td>
    <td colspan='3'>Date</td>
    <td style="width: 184px"></td>
    <td style="width: 120px">Date</td>
    <td style="width: 321px"></td>
    <td>Date</td>
      <td>
      </td>
    <td></td>
    <td colspan='2'>Date</td>
    <td style="width: 26px"></td>
  </tr>
</table></div>
 <div id="print"  style=" width:59px; height:33px; color:Gray;cursor:pointer" onclick="javascirpt:print('printview','Report_print','<% response.write(Session("company_name")) %>','<% response.write(Today.ToLongDateString) %>');">
 <img src='images/ico/print.ico' alt="print"/>print</div>        
<%  Dim flg As Integer = 0
    ds.save("begin transaction", Session("con"), Session("path"))
    
    For i = 0 To sql.Length - 1
        If String.IsNullOrEmpty(sql(i)) = False Then
            flg = ds.save(sql(i), Session("con"), Session("path"))
            If flg = -2 Then
                Exit For
            End If
        End If
        'Response.Write(sql(i) & "<br>")
    Next
    If flg = -2 Then
        Response.Write("Data is not save")
        ds.save("rollback", Session("con"), Session("path"))
    Else
        ds.save("commit", Session("con"), Session("path"))
        %>
            <script type="text/javascript">
            document.getElementById("refid").innerHTML="Ref. No.:<% response.write(ref) %>";
               // alert("<% response.write("Payment Reference is:" & ref & "\nThis all information you can get viewing the information again") %>");
            </script>
        
        <%
        End If
    End If
ElseIf Request.QueryString("save") = "onmultiple" Then
    
   
    rtn = gosaved()
    
    t2 = Now
    Dim timout As String = ""
    timout = t2.Subtract(t1).Minutes & "Mins " & t2.Subtract(t1).Seconds & "Secs"
    If rtn <> "" Then
      %>
        <script>            $("#result").text("<%=rtn %>");   </script>
      <%
    End If
    'Response.Write("<div id='result'>" & rtn & "<br> End with: " & timout & "</div>")
End If
    %>
</body>
</html>
