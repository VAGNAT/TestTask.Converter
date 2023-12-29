import { v4 as uuidv4 } from "uuid";
import React, { useState, useEffect } from "react";
import axios from "axios";
import Cookies from "js-cookie";
import { createAxiosInstance } from "../API/CreateAxios";
import {
  Button,
  List,
  ListItem,
  ListItemText,
  CircularProgress,
} from "@mui/material";
import { styled } from "@mui/material/styles";

const Input = styled("input")({
  display: "none",
});

const UploadLabel = styled("label")(({ theme }) => ({
  backgroundColor: theme.palette.primary.main,
  color: theme.palette.primary.contrastText,
  padding: "8px 20px",
  borderRadius: "4px",
  cursor: "pointer",
  "&:hover": {
    backgroundColor: theme.palette.primary.dark,
  },
  marginTop: theme.spacing(2),
}));

const FilesPage = () => {
  const [files, setFiles] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const axiosInstance = createAxiosInstance();
  axios.defaults.withCredentials = true;

  useEffect(() => {
    const sessionId = Cookies.get("sessionId");
    if (!sessionId) {
      const newGuid = uuidv4();
      Cookies.set("sessionId", newGuid);
    }
    fetchFiles();
  }, []);

  // Fetch the list of files from the server
  const fetchFiles = async () => {
    setIsLoading(true);
    try {
      const response = await axiosInstance.get(`FileManager`);
      setFiles(response.data);
    } catch (error) {
      console.error("Failed to fetch files:", error);
    } finally {
      setIsLoading(false);
    }
  };

  // Upload file function
  const handleFileUpload = async (event) => {
    const file = event.target.files[0];
    if (file && file.type === "text/html") {
      const formData = new FormData();
      formData.append("file", file);

      try {
        const response = await axiosInstance.post(`FileManager`, formData, {
          headers: {
            "Content-Type": `multipart/form-data`,
          },
        });
        if (response.data && response.data.fileId) {
          console.log(`File uploaded with ID: ${response.data.fileId}`);
        }
        fetchFiles();
      } catch (error) {
        console.error("Failed to upload file:", error);
      }
    } else {
      alert("Please select a file with .html extension");
    }
  };

  // Delete file function
  const handleFileDelete = async (fileId) => {
    try {
      await axiosInstance.delete(`FileManager/${fileId}`);
      fetchFiles(); // Refresh file list
    } catch (error) {
      console.error("Failed to delete file:", error);
    }
  };

  // Download file function
  const handleFileDownload = async (file) => {
    try {
      const response = await axiosInstance.get(`FileManager/${file.id}`, {
        responseType: "blob",
      });

      if (response.status === 202) {
        alert("File is still processing. Please try again later.");
        return;
      }

      const contentType = response.headers["content-type"];

      const fileNameWithoutExtension = file.name
        .split(".")
        .slice(0, -1)
        .join(".");
      const url = window.URL.createObjectURL(
        new Blob([response.data], { type: contentType })
      );
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", fileNameWithoutExtension);
      document.body.appendChild(link);
      link.click();

      window.URL.revokeObjectURL(url);
      link.parentNode.removeChild(link);
    } catch (error) {
      if (error.response && error.response.status === 202) {
        alert("File is still processing. Please try again later.");
      } else {
        console.error("Failed to download file:", error);
      }
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <div>
        <Input
          accept=".html"
          id="contained-button-file"
          multiple
          type="file"
          onChange={handleFileUpload}
        />
        <UploadLabel htmlFor="contained-button-file">
          Upload HTML File
        </UploadLabel>
      </div>

      {isLoading ? (
        <CircularProgress />
      ) : (
        <List>
          {files.map((file) => (
            <ListItem
              key={file.id}
              sx={{
                border: "1px solid #ddd",
                my: 1,
                borderRadius: "4px",
              }}
            >
              <ListItemText primary={file.name} />
              <Button
                variant="contained"
                color="primary"
                sx={{ mx: 1 }}
                onClick={() => handleFileDownload(file)}
              >
                Download
              </Button>
              <Button
                variant="contained"
                color="secondary"
                sx={{ mx: 1 }}
                onClick={() => handleFileDelete(file.id)}
              >
                Delete
              </Button>
            </ListItem>
          ))}
        </List>
      )}
    </div>
  );
};

export default FilesPage;
