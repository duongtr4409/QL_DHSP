import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  DanhSachBieuMau: [],
  TotalRow: 0,
  TableLoading: false,
  FileLimit: 1,
  FileAllow: ""
};

export default function Reducer(state = initState, action){
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.BIEUMAU_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role
      };
    case actions.BIEUMAU_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachBieuMau: payload.DanhSachBieuMau,
        TotalRow: payload.TotalRow,
        TableLoading: false,
        FileLimit: payload.FileLimit,
        FileAllow: payload.FileAllow
      };
    case actions.BIEUMAU_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        DanhSachBieuMau: [],
        TotalRow: 0,
        TableLoading: false,
        FileLimit: 1,
        FileAllow: ""
      };
    //get list
    case actions.BIEUMAU_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.BIEUMAU_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachBieuMau: payload.DanhSachBieuMau,
        TotalRow: payload.TotalRow,
        TableLoading: false
      };
    case actions.BIEUMAU_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        DanhSachBieuMau: [],
        TotalRow: 0,
        TableLoading: false
      };
    default:
      return state;
  }
}