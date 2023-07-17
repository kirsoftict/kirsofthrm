<%@ Page Language="VB" AutoEventWireup="false" CodeFile="~/previewpring.aspx.vb" Inherits="css_previewpring" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
    <title>Untitled Page</title>
    <style>
        body {
        margin: 0;
        padding: 0;
        background-color: #FAFAFA;
        font: 10pt;
    }
    * {
        box-sizing: border-box;
        -moz-box-sizing: border-box;
    }
    .page {
        width: 18cm;
        min-height: 20.7cm;
        padding: 0.5cm;
        margin: 0.5cm auto;
        border: 1px #D3D3D3 solid;
        border-radius: 1px;
        background: white;
        box-shadow: 0 0 1px rgba(0, 0, 0, 0.1);
    }
    .subpage {
        padding: 0.5cm;
        border: 1px red solid;
        
        outline: 1cm #FFEAEA solid;
    }
    
    @page {
        size: A4;
        margin: 0;
    }
    @media print {
        .page {
            margin: 0;
            border: initial;
            border-radius: initial;
            width: initial;
            min-height: initial;
            box-shadow: initial;
            background: initial;
            page-break-after: always;
        }
    }
    </style>
    
    <script>window.print();</script>
</head>
<body>
    <div class="book">
    <div class="page">
        <div class="subpage">Page 1/2<br />
           <div id='showd'></div>
        </div>    
    </div>
    <div class="page">
        <div class="subpage">Page 2/2</div>    
    </div>
    <%  Response.Write(Request.QueryString("send"))
        %>
</div>
</body>
</html>
<script language="javascript" type="text/javascript">
   //var jvel=new <% Response.Write(Request.QueryString("send"))%>;
   // var el=document.getElementById("showd");
   // var name=jvel.innerHTML;
   // el.innerHTML=name;
</script>
