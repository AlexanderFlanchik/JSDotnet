async function hanlder() {
    const html = `
        <html>
            <head>
                <title>Thank you!</title>
                <link rel="stylesheet" type="text/css" href="/css/info-submitted@0.0.1.css" />
            </head>
            <body>
                <div class="wrapper">
                    <div class="main-content">
                        <span>Done. Your data has been submitted. </span>
                    </div>
                </div>
            </body>
        </html>
    `;

    callback(html, null, "text/html");
}

hanlder();
