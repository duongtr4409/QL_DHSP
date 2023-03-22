import {
  apiGetAuth, apiPostAuth, apiPostAuth2
} from "../../../api";
import server from '../../../settings';

const apiUrl = {
  // uploadimage: server.apiImage + 'ocr/recognition',
  // verification: server.apiImage + 'face/verification',

  uploadimage: server.apiImage + 'ekyc/recognition',
  verification: server.apiImage + 'ekyc/verification',

  checkin: server.apiUrl + 'VaoRa/Vao',
  checkout: server.apiUrl + 'VaoRa/Ra',
  danhsachcanbo: server.apiUrl + 'HeThongCanBo/DanhSachCanBo_TrongCoQuanSuDungPhanMem',
  danhsachletan: server.apiUrl + 'HeThongCanBo/GetDanhSachLeTan',
  getbymathe: server.apiUrl + 'VaoRa/GetByMaThe',
  getbychandung: server.apiUrl + 'VaoRa/Get_By_AnhChanDung',
  getimagecamera: server.apiUrl + 'VaoRa/Get_AnhChanDung_FromCamera',
  getipcamera: server.apiUrl + 'SystemConfig/GetByKey'
};
const api = {
  UploadImage: (param) => {
    return apiPostAuth2(apiUrl.uploadimage, {...param});
  },
  Checkin: (param) => {
    return apiPostAuth(apiUrl.checkin, {...param});
  },
  Checkout: (param) => {
    return apiPostAuth(apiUrl.checkout, {...param});
  },
  DanhSachCanBo: () => {
    return apiGetAuth(apiUrl.danhsachcanbo);
  },
  DanhSachLeTan: () => {
    return apiGetAuth(apiUrl.danhsachletan);
  },
  GetByMaThe: (param) => {
    return apiGetAuth(apiUrl.getbymathe, {...param});
  },
  Verification: (param) => {
    return apiPostAuth2(apiUrl.verification, {...param});
  },
  GetByChanDung: (param) => {
    return apiPostAuth(apiUrl.getbychandung, {...param});
  },
  GetImageCamera: (param) => {
    return apiGetAuth(apiUrl.getimagecamera, {...param});
  },
  GetIPCamera: (param) => {
    return apiGetAuth(apiUrl.getipcamera, {...param});
  },
};

export default api;