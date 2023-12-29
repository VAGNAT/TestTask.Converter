import React, { useState } from "react";
import axios from "axios";
import {
  Button,
  Container,
  List,
  ListItem,
  ListItemText,
  TextField,
} from "@mui/material";

const FileList = () => {
  const [files, setFiles] = useState([]);
  const [selectedFile, setSelectedFile] = useState(null);

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    setSelectedFile(file);
  };

  const handleFileUpload = async () => {
    if (selectedFile && selectedFile.name.endsWith(".html")) {
      const formData = new FormData();
      formData.append("file", selectedFile);

      try {
        const response = await axios.post("your-backend-api/upload", formData);
        const { fileId, fileStatus } = response.data;

        setFiles((prevFiles) => [
          ...prevFiles,
          { id: fileId, name: selectedFile.name, status: fileStatus },
        ]);
      } catch (error) {
        console.error("Error uploading file:", error);
      }
    } else {
      console.error("Invalid file type. Please select an HTML file.");
    }
  };

  const handleFileDelete = (fileId) => {
    // Implement file deletion logic here
    // Update the 'files' state accordingly
  };

  const handleFileDownload = async (fileId) => {
    // Implement file download logic here
    // You may use axios to download the file from the backend
  };

  return (
    <Container>
      <input type="file" onChange={handleFileChange} />
      <Button variant="contained" onClick={handleFileUpload}>
        Upload
      </Button>
      <List>
        {files.map((file) => (
          <ListItem key={file.id}>
            <ListItemText
              primary={file.name}
              secondary={`Status: ${file.status}`}
            />
            <Button
              variant="outlined"
              onClick={() => handleFileDownload(file.id)}
            >
              Download
            </Button>
            <Button
              variant="outlined"
              onClick={() => handleFileDelete(file.id)}
            >
              Delete
            </Button>
          </ListItem>
        ))}
      </List>
    </Container>
  );
};

export default FileList;
