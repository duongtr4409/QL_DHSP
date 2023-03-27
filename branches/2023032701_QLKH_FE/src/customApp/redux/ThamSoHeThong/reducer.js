import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  TotalRow: 0,
  TableLoading: false,
  DanhSachThamSo: []
};

export default function Reducer(state = initState, action){
  const {type, payload} = action;
  switch (type) {
    case actions.THAMSO_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role
      };
    case actions.THAMSO_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        TotalRow: payload.TotalRow,
        DanhSachThamSo: payload.DanhSachThamSo,
        TableLoading: false
      };
    case actions.THAMSO_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        TotalRow: 0,
        TableLoading: false
      };
    //get list
    case actions.THAMSO_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.THAMSO_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        TotalRow: payload.TotalRow,
        DanhSachThamSo: payload.DanhSachThamSo,
        TableLoading: false
      };
    case actions.THAMSO_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        TotalRow: 0,
        TableLoading: false
      };
    default:
      return state;
  }
}