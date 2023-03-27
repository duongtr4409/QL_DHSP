import { take, call, put, select, takeLatest } from "redux-saga/effects";

// Individual exports for testing
import * as actions from "./actions";
import * as constants from "./constants";
import api from "../../containers/QLDeTai/config";
function* getList({ payload }) {
  try {
    const response = yield call(api.danhSachDeTai, payload.filterData);

    yield put({
      type: constants.GET_DETAI_ACTION_SUCCESS,
      payload: {
        listDeTai: response.data.Data,
        TotalRow: response.data.TotalRow,
      },
    });
  } catch (e) {
    yield put({
      type: constants.GET_DETAI_ACTION_ERROR,
      e,
    });
  }
}

export default function* QLDeTaiSaga() {
  // See example in containers/HomePage/saga.js
  yield takeLatest(constants.GET_DETAI_ACTION, getList);
}
