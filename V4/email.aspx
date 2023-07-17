<%@ Page Language="VB" AutoEventWireup="false" CodeFile="email.aspx.vb" Inherits="email" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="emailform"  name='emailform' method="post">
    <div>
       Subject: <input type="text" id='subject' name='subject'  /><br />
        To: <input type='text' id='to' name='to'  />
          <input type='text' id='from' name='from' />
            <input type='text' id='port' name='port' value='587' />
            <input type'text' id='smtp' name='smtp' value='smtp.gmail.com'  />
             <input type'text' id='demail' name='demail' value='kirsoftet@gmail.com'  />
              <input type'text' id='dto' name='dto' value='z.kirubel@gmail.com'  />
               <textarea id='msg' name='msg' rows=10 cols=100></textarea>
               <input type=submit name=Send />

    </div>
    </form>
    
</body>
</html>
