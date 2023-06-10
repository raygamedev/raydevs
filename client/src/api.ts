import axios from 'axios';

export const downloadFile = (event: { preventDefault: () => void }) => {
  event.preventDefault();
  axios
    .get('/download')
    .then((response) => {
      // Create a download link element and click it to initiate the download
      const link = document.createElement('a');
      link.href = URL.createObjectURL(new Blob([response.data]));
      link.download = 'dan_raymond_resume.pdf';
      link.click();
      URL.revokeObjectURL(link.href);
    })
    .catch((error) => {
      console.error('Error downloading the file:', error);
    });
};
