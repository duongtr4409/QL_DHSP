import actions from '../../redux/NhacViec/actions';

const initState = {
  notifications: [],
  TotalRow: 0,
  TableLoading: false
};

export default function Reducer(state = initState, action) {
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.NHACVIEC_GET_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
      };
    case actions.NHACVIEC_GET_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        notifications: payload.notifications,
        TotalRow: payload.TotalRow,
        TableLoading: false
      };
    case actions.NHACVIEC_GET_DATA_REQUEST_ERROR:
      return {
        ...state,
        notifications: [],
        TotalRow: 0,
        TableLoading: false
      };
    default:
      return state;
  }
}