import { take, call, put, select, takeLatest } from "redux-saga/effects";

// Individual exports for testing
import * as actions from "./actions";
import * as constants from "./constants";
import api from "../../containers/QLDeXuat/config";
function* getList({ payload }) {
  try {
    const response = yield call(api.danhSachDeXuat, payload.filterData);

    yield put({
      type: constants.GET_DEXUAT_ACTION_SUCCESS,
      payload: {
        listDeXuat: response.data.Data,
        TotalRow: response.data.TotalRow,
      },
    });
  } catch (e) {
    yield put({
      type: constants.GET_DEXUAT_ACTION_ERROR,
      e,
    });
  }
}

export default function* QLDeXuatSaga() {
  // See example in containers/HomePage/saga.js
  yield takeLatest(constants.GET_DEXUAT_ACTION, getList);
}
