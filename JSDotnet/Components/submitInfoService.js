//========================
// This component represents some business logic which requires further redirect to another page.
//========================
async function hanlder() {
    // Doing something important
    context.Log(`Log in JS handler, received: ${dataJson}`);
    callback(null, "/info-submitted", null);
}

hanlder();