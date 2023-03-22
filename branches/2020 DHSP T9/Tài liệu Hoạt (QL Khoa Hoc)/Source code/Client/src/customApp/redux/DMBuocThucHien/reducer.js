import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  DanhSachBuocThucHien: [],
  TotalRow: 0,
  TableLoading: false,
};

export default function Reducer(state = initState, action){
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.BUOCTHUCHIEN_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role
      };
    case actions.BUOCTHUCHIEN_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachBuocThucHien: payload.DanhSachBuocThucHien,
        TotalRow: payload.TotalRow,
        TableLoading: false,
      };
    case actions.BUOCTHUCHIEN_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        DanhSachBuocThucHien: [],
        TotalRow: 0,
        TableLoading: false,
      };
    //get list
    case actions.BUOCTHUCHIEN_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.BUOCTHUCHIEN_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachBuocThucHien: payload.DanhSachBuocThucHien,
        TotalRow: payload.TotalRow,
        TableLoading: false
      };
    case actions.BUOCTHUCHIEN_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        DanhSachBuocThucHien: [],
        TotalRow: 0,
        TableLoading: false
      };
    default:
      return state;
  }
}