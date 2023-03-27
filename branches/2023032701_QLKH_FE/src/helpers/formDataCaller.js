import axios from "axios";
export const formDataCaller = (url, data) => {
  return axios({
    method: "POST",
    url,
    data,

    headers: {
      "Content-Type": "multipart/form-data",
      Authorization: `Bearer ${localStorage.getItem("access_token")}`,
    },
  }).catch((err) => {
    // console.log(err.response);
    const { status, statusText } = err.response;
    return {
      Status: 0,
      Message: status || statusText ? `${status} - ${statusText}` : "Đã có lỗi xảy ra",
    };
  });
};
