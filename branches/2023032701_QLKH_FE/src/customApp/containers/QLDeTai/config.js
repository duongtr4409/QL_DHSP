import { apiGetAuth, apiGetUser, apiPostAuth } from "../../../api";
import server from "../../../settings";
import { getDefaultPageSize } from "../../../helpers/utility";
import { formDataCaller } from "../../../helpers/formDataCaller";

export const apiUrl = {
  danhsachKQNC: server.apiInOut + "PhanQuyen/GetListPaging",
  danhsachloaihinhnghiencuu: server.apiUrl + "DanhMucLoaiHinhNghienCuu/GetAll",
  caylinhvuc: server.apiUrl + "DanhMucLinhVuc/GetAllAndGroup",
  danhsachcapdetai: server.apiUrl + "DanhMucCapDeTai/GetAllAndGroup",
  danhsachloainhiemvuchoKQNC: server.apiUrl + "DataInCore/DSNhiemVuKhoaHoc",
  danhsachtaikhoan: server.apiUrl + "DataInCore/getstave",
  getConversions: server.apiUrl + "DataInCore/getConversions",
  danhsachcanbotrongtruong: server.apiUrl + "HeThongCanBo/GetListPaging",
  thongtincanbo: server.apiUrl + "DeTai/DeTai_ThongTinCanBo",
  themdetai: server.apiUrl + "DeTai/DeTai_ThemMoi",
  danhsachdetai: server.apiUrl + "DeTai/DeTai_DanhSach",
  chitietdetai: server.apiUrl + "DeTai/DeTai_ChiTiet",
  capnhatdetai: server.apiUrl + "DeTai/DeTai_CapNhat",
  capnhattrangthaidetai: server.apiUrl + "DeTai/DeTai_CapNhatTrangThai",
  laycapdetaitheoid: server.apiUrl + "DanhMucCapDeTai/GetByID",
  chinhsuathongtinchitiet: server.apiUrl + "DeTai/DeTai_ChinhSuaThongTinChiTiet",
  xoathongtinchitiet: server.apiUrl + "DeTai/DeTai_XoaThongTinChiTiet",
  themfiledinhkem: server.apiUrl + "DeXuatDeTai/ThemMoiFileDinhKem",
  danhsachloaiketqua: server.apiUrl + "DanhMucLoaiKetQua/GetAll",
  chinhsuaketquanghiencuu: server.apiUrl + "DeTai/DeTai_ChinhSuaKetQuaNghienCuu",
  xoaketquanghiencuu: server.apiUrl + "DeTai/DeTai_XoaKetQuaNghienCuu",
  xoadetai: server.apiUrl + "DeTai/DeTai_XoaDeTai",
  danhsachallcanbo: server.apiUrl + "HeThongCanBo/GetListPaging",
  xoafiledinhkem: server.apiUrl + "DeXuatDeTai/XoaFileDinhKem",
  themnhakhoahoc: server.apiUrl + "LyLichKhoaHoc/NhaKhoaHoc_ThemMoi",
};
const api = {
  danhSachKQNC: (param) => {
    return apiGetAuth(apiUrl.danhsachKQNC, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize(),
    });
  },
  danhSachLoaiHinhNghienCuu: () => {
    return apiGetAuth(apiUrl.danhsachloaihinhnghiencuu);
  },
  danhSachCayLinhVuc: (param) => {
    return apiGetAuth(apiUrl.caylinhvuc, {
      ...param,
    });
  },
  danhSachCapDeTai: (param) => {
    return apiGetAuth(apiUrl.danhsachcapdetai, {
      ...param,
    });
  },
  danhSachLoaiNhiemVuChoKQNC: (param) => {
    return apiGetAuth(apiUrl.danhsachcapdetai, {
      ...param,
    });
  },
  DanhSachTaiKhoan: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachtaikhoan, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize(),
    });
  },
  danhSachCanBoTrongTruong: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachtaikhoan, {
      ...param,
    });
  },
  danhSachAllCanBo: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachallcanbo, {
      ...param,
    });
  },
  danhSachVaiTroTheoNhiemVu: (param = {}) => {
    return apiGetAuth(apiUrl.getConversions, { ...param });
  },
  thongTinCanBoTheoMa: (param) => {
    return apiGetAuth(apiUrl.thongtincanbo, {
      ...param,
    });
  },
  themDeTai: (data) => {
    return formDataCaller(apiUrl.themdetai, data);
  },
  danhSachDeTai: (param) => {
    return apiGetAuth(apiUrl.danhsachdetai, {
      ...param,
      Role: localStorage.getItem("role_id"),
    });
  },
  chiTietDeTai: (param = {}) => {
    return apiGetAuth(apiUrl.chitietdetai, { ...param });
  },
  danhMucCapDeTaiByID: (param = {}) => {
    return apiGetAuth(apiUrl.laycapdetaitheoid, { ...param });
  },
  capNhatDeTai: (param = {}) => {
    return apiPostAuth(apiUrl.capnhatdetai, { ...param });
  },
  capNhatTrangThaiDeTai: (param = {}) => {
    return apiPostAuth(apiUrl.capnhattrangthaidetai, { ...param });
  },
  chinhSuaThongTinChiTiet: (param = {}, isFormData = false) => {
    if (!isFormData) {
      return apiPostAuth(apiUrl.chinhsuathongtinchitiet, { ...param });
    }
    return formDataCaller(apiUrl.themdetai, param);
  },
  xoaThongTinChiTiet: (param = {}) => {
    return apiPostAuth(apiUrl.xoathongtinchitiet, { ...param });
  },
  themFileDinhKem: (data) => {
    return formDataCaller(apiUrl.themfiledinhkem, data);
  },
  xoaFileDinhKem: (param) => {
    return apiPostAuth(apiUrl.xoafiledinhkem, { ...param });
  },
  danhMucLoaiKetQua: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachloaiketqua, { ...param });
  },
  chinhSuaKetQuaNghienCuu: (param = {}, isFormData = false) => {
    return apiPostAuth(apiUrl.chinhsuaketquanghiencuu, { ...param });
  },
  xoaKetQuaNghienCuu: (param = {}) => {
    return apiPostAuth(apiUrl.xoaketquanghiencuu, { ...param });
  },
  xoaDeTai: (param = {}) => {
    return apiPostAuth(apiUrl.xoadetai, { ...param });
  },
};

export default api;
