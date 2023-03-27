import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  roleDuyet: {view: 0, add: 0, edit: 0, delete: 0},
  DanhSachThuyetMinh: [],
  DanhSachCapQuanLy: [],
  DanhSachCanBo: [],
  TableLoading: false,
  FileAllow: "",
  FileLimit: 0,
};

export default function Reducer(state = initState, action){
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.THUYETMINH_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role,
        roleDuyet: payload.roleDuyet,
      };
    case actions.THUYETMINH_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        TableLoading: false,
        DanhSachThuyetMinh: payload.DanhSachThuyetMinh,
        DanhSachCapQuanLy: payload.DanhSachCapQuanLy,
        DanhSachCanBo: payload.DanhSachCanBo,
        FileAllow: payload.FileAllow,
        FileLimit: payload.FileLimit,
      };
    case actions.THUYETMINH_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        TableLoading: false,
        DanhSachThuyetMinh: [],
        DanhSachCapQuanLy: [],
        DanhSachCanBo: [],
        FileAllow: "",
        FileLimit: 0,
      };
    //get list
    case actions.THUYETMINH_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.THUYETMINH_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        TableLoading: false,
        DanhSachThuyetMinh: payload.DanhSachThuyetMinh
      };
    case actions.THUYETMINH_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        TableLoading: false,
        DanhSachThuyetMinh: []
      };
    default:
      return state;
  }
}