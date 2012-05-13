<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Index</title>
    <script type="text/javascript" src="../../Scripts/jquery-1.5.1.js"></script>
    <script type="text/javascript" src="../../Scripts/json2.js"></script>
    <script type="text/javascript">
        function getPerson() {
            var name = $("#Name").val();
            var age = $("#Age").val();

            // poor man's validation
            return (name == "") ? null : { Name: name, Age: age };
        }

        $(function () {
            $("#personCreate").click(function () {
                var person = getPerson();

                // poor man's validation
                if (person == null) {
                    alert("Specify a name please!");
                    return;
                }

                var json = JSON.stringify(person);

                $.ajax({
                    url: '/ajax/save',
                    type: 'POST',
                    dataType: 'json',
                    data: json,
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        // get the result and do some magic with it
                        var message = data.Message;
                        alert(data);

                        $("#resultMessage").html(message);
                    }
                });
            });
        });

    </script>
</head>

<body>
    <div>
        <h1>HEllo World</h1>   
        <div>Name:</div>
        <input id="Name" type="text" />
        <br />

        <div>Age:</div>
        <input id="Age" type="text" />
        <br />
        <br />

        <input id="personCreate" type="button" value="Submit!" />
        <br />
        <br />

        <div id="resultMessage"></div>
    </div>
</body>
</html>
