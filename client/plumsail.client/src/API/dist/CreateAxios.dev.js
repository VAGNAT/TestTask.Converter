"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.createAxiosInstance = void 0;

var _axios = _interopRequireDefault(require("axios"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

var createAxiosInstance = function createAxiosInstance() {
  var axiosInstance = _axios["default"].create({
    baseURL: process.env.REACT_APP_BACKEND_API_URL,
    withCredentials: true,
    headers: {
      "Access-Control-Allow-Origin": "*",
      "Content-Type": "application/json",
      mode: "cors"
    }
  });

  return axiosInstance;
};

exports.createAxiosInstance = createAxiosInstance;