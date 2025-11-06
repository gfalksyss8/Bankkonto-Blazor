
/*
window.downloadFile = (fileName, base64Data) => {
    const link = document.createElement('a');
    link.href = "data:application/octet-stream;base64," + base64Data;
    link.download = fileName;
    link.click();
};
*/

window.downloadFile = (fileName, base64Data) => {
    if (!fileName) {
        console.warn("downloadFile: fileName is null or empty. Defaulting to 'download.bin'");
        fileName = "download.bin";
    }
    const link = document.createElement('a');
    link.href = "data:application/octet-stream;base64," + base64Data;
    link.download = fileName;
    document.body.appendChild(link); // ensure in DOM
    link.click();
    document.body.removeChild(link);
};

