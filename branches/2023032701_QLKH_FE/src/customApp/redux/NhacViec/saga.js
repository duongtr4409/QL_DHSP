import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../../containers/Topbar/config";
import actions from "./actions";

function* getNotifications({payload}) {
  try {
    const response = yield call(api.getNotifications, payload.filterData);
    //return action
    yield put({
      type: actions.NHACVIEC_GET_DATA_REQUEST_SUCCESS,
      payload: {
        notifications: response.data.Data,
        TotalRow: response.data.TotalRow
      }
    });
  } catch (e) {
    yield put({
      type: actions.NHACVIEC_GET_DATA_REQUEST_ERROR
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.NHACVIEC_GET_DATA_REQUEST, getNotifications)]);
}
