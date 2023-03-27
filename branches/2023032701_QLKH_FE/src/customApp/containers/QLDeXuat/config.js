import { apiGetAuth, apiGetUser, apiPostAuth } from "../../../api";
import server from "../../../settings";
import { getDefaultPageSize } from "../../../helpers/utility";
import { formDataCaller } from "../../../helpers/formDataCaller";

const apiUrl = {
  danhsachKQNC: server.apiInOut + "PhanQuyen/GetListPaging",
  danhsachdexuat: server.apiUrl + "DeXuatDeTai/DeXuatDeTai_DanhSach",
  themdexuat: server.apiUrl + "DeXuatDeTai/DeXuatDeTai_ThemMoi",
  capnhatdexuat: server.apiUrl + "DeXuatDeTai/DeXuatDeTai_CapNhat",
  capnhattrangthai: server.apiUrl + "DeXuatDeTai/DeXuatDeTai_CapNhatTrangThai",
  themfiledinhkem: server.apiUrl + "DeXuatDeTai/ThemMoiFileDinhKem",
  xoafiledinhkem: server.apiUrl + "DeXuatDeTai/XoaFileDinhKem",
  chitietdexuat: server.apiUrl + "DeXuatDeTai/DeXuatDeTai_ChiTiet",
  danhsachtaikhoan: server.apiUrl + "DataInCore/getstave",
  caycapquanly: server.apiUrl + "DanhMucCapDeTai/GetAllAndGroup",
  caylinhvuc: server.apiUrl + "DanhMucLinhVuc/GetAllAndGroup",
  danhsachtrangthai: server.apiUrl + "DanhMucTrangThai/GetAll",
  xoadexuat: server.apiUrl + "DeXuatDeTai/DeXuatDeTai_XoaDeXuat",
  chitietcapquanly: server.apiUrl + "DanhMucCapDeTai/GetByID",
};
const api = {
  danhSachKQNC: (param) => {
    return apiGetAuth(apiUrl.danhsachKQNC, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize(),
    });
  },
  danhSachDeXuat: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachdexuat, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize(),
      Role: localStorage.getItem("role_id"),
    });
  },
  themDeXuat: (data) => {
    return formDataCaller(apiUrl.themdexuat, data);
  },
  themFileDinhKem: (data) => {
    return formDataCaller(apiUrl.themfiledinhkem, data);
  },
  xoaFileDinhKem: (param) => {
    return apiPostAuth(apiUrl.xoafiledinhkem, {
      ...param,
    });
  },
  chiTietDeXuat: (param) => {
    return apiGetAuth(apiUrl.chitietdexuat, { ...param });
  },
  capNhatDeXuat: (param) => {
    return apiPostAuth(apiUrl.capnhatdexuat, { ...param });
  },
  capNhatTrangThaiDeXuat: (param) => {
    return apiPostAuth(apiUrl.capnhattrangthai, { ...param });
  },
  DanhSachTaiKhoan: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachtaikhoan, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize(),
    });
  },
  danhSachCayCapQuanly: (param) => {
    return apiGetAuth(apiUrl.caycapquanly, {
      ...param,
    });
  },
  danhSachCayLinhVuc: (param) => {
    return apiGetAuth(apiUrl.caylinhvuc, {
      ...param,
    });
  },
  danhSachTrangThai: (param) => {
    return apiGetAuth(apiUrl.danhsachtrangthai, {
      ...param,
    });
  },
  xoaDeXuat: (param) => {
    return apiPostAuth(apiUrl.xoadexuat, {
      ...param,
    });
  },
  chiTietCapQuanly: (param) => {
    return apiGetAuth(apiUrl.chitietcapquanly, {
      ...param,
    });
  },
};

export default api;
export const TrangThai = [
  // {
  //   label: "Chưa gửi",
  //   value: 0,
  // },
  {
    label: "Chưa gửi",
    value: 1,
  },
  {
    label: "Chưa duyệt",
    value: 2,
  },
  {
    label: "Duyệt",
    value: 4,
    isDicision: true,
  },
  {
    label: "Duyệt phải sửa",
    value: 3,
    isDicision: true,
  },

  {
    label: "Không duyệt",
    value: 5,
    isDicision: true,
  },

  {
    label: "Chờ duyệt",
    value: 6,
    isDicision: true,
  },
];
