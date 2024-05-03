//====================
// Render function called from .NET
// dataJson & callback are parameters passed to this script
//====================
async function render() {
    const data = JSON.parse(dataJson);
    const afTokenInputName = data.antiforgeryTokenFieldName,
        afTokenValue = data.antiforgeryTokenValue;

    const html = `
        <html>
            <head>
                <title>I'm home!</title>
                <link rel="stylesheet" href="/css/app.css" type="text/css" />
            </head>
            <body>
                <div class="wrapper">
                   <div class="form-holder">
                        <div class="form-header">
                            Please provide additional info:
                        </div>
                        <form action="/submit-info" method="POST">
                        <input type="hidden" name="${afTokenInputName}" value="${afTokenValue}" />
                        <div class="form-row">
                            <div>First Name:</div>
                            <div><input name="firstName" /></div>
                        </div>
                        <div class="form-row">
                            <div>Last Name:</div>
                            <div><input name="lastName" /></div>
                        </div>
                        <div class="form-row">
                            <div>Phone Number:</div>
                            <div><input name="phoneNumber" /></div>
                        </div>
                        <div class="button-bar">
                            <button class="btn btn-primary" role="submit">Submit..</button>
                        </div>
                        </form>
                   </div>
                </div>
            </body>
        </html>
    `;

    callback(html, null, 'text/html');
}

render();