import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  roleQL: {view: 0, add: 0, edit: 0, delete: 0},
  TableLoading: false,
  DanhSachChucDanh: [],
  DanhSachPhongBan: [],
  DanhSachHocVi: [],
  DanhSachNhiemVu: [],
  DanhSachNhiemVuKhoaHoc: [],
  DanhSachDeTai: [],
  FileLimit: 1,
  FileAllow: "",
  DanhSachLoaiNhiemVu: [],
  DanhSachCanBo: [],
  CanBoNgoaiTruong: null,
  DanhSachLoaiKetQua: []
};

export default function Reducer(state = initState, action) {
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role,
        roleQL: payload.roleQL,
      };
    case actions.CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        TableLoading: false,
        DanhSachChucDanh: payload.DanhSachChucDanh,
        DanhSachPhongBan: payload.DanhSachPhongBan,
        DanhSachHocVi: payload.DanhSachHocVi,
        DanhSachNhiemVu: payload.DanhSachNhiemVu,
        DanhSachNhiemVuKhoaHoc: payload.DanhSachNhiemVuKhoaHoc,
        DanhSachDeTai: payload.DanhSachDeTai,
        FileLimit: payload.FileLimit,
        FileAllow: payload.FileAllow,
        DanhSachLoaiNhiemVu: payload.DanhSachLoaiNhiemVu,
        DanhSachCanBo: payload.DanhSachCanBo,
        CanBoNgoaiTruong: payload.CanBoNgoaiTruong,
        DanhSachLoaiKetQua: payload.DanhSachLoaiKetQua,
      };
    case actions.CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        TableLoading: false,
        DanhSachChucDanh: [],
        DanhSachPhongBan: [],
        DanhSachChucVu: [],
        DanhSachNhiemVu: [],
        DanhSachNhiemVuKhoaHoc: [],
        DanhSachDeTai: [],
        FileLimit: 1,
        FileAllow: "",
        DanhSachLoaiNhiemVu: [],
        DanhSachCanBo: [],
        CanBoNgoaiTruong: null,
        DanhSachLoaiKetQua: []
      };
    //get list
    case actions.CHITIETNHAKHOAHOC_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.CHITIETNHAKHOAHOC_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        TableLoading: false
      };
    case actions.CHITIETNHAKHOAHOC_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        TableLoading: false
      };
    default:
      return state;
  }
}