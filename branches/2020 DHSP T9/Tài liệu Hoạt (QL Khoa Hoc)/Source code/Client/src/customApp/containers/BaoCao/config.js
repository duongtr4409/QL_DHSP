import { apiGetAuth, apiPostAuth } from "../../../api";
import server from "../../../settings";

const apiUrl = {
  danhsachloaihinhnghiencuu: server.apiUrl + "DanhMucLoaiHinhNghienCuu/GetAll",
  caylinhvuc: server.apiUrl + "DanhMucLinhVuc/GetAllAndGroup",
  danhsachcapdetai: server.apiUrl + "DanhMucCapDeTai/GetAllAndGroup",
  danhsachhochamhocvi: server.apiUrl + "DataInCore/getDegrees",
  danhsachdonvicongtac: server.apiUrl + "DataInCore/getDepartments",
  danhsachchucvu: server.apiUrl + "DataInCore/getPositions",
  thongkekqnc: server.apiUrl + "BaoCaoThongKe/BCThongKeKetQuaNghienCuu",
  hoatdongnckh: server.apiUrl + "BaoCaoThongKe/BCThongKeHoatDongNghienCuu",
  thongkenvkh: server.apiUrl + "BaoCaoThongKe/BCThongKeNhiemVuKhoaHoc",
  danhsachnvkh: server.apiUrl + "BaoCaoThongKe/BCDanhSachNhiemVuKhoaHoc",
  tinhhinhvakqthuchiennhiemvu: server.apiUrl + "BaoCaoThongKe/BCTinhHinhKetQuaThucHien",
  congboketqua: server.apiUrl + "BaoCaoThongKe/CongBoKetQuaNghienCuu",
  thongkebctheochisoISSN: server.apiUrl + "BaoCaoThongKe/BCKeKhaiBaiBaoKhoaHoc",
};
const api = {
  TaoBaoCao: (param) => {
    return apiGetAuth(apiUrl.taobaocao, { ...param });
  },
  danhSachLoaiHinhNghienCuu: (param) => {
    return apiGetAuth(apiUrl.danhsachloaihinhnghiencuu, { ...param });
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
  danhSachHocHamHocVi: (param) => {
    return apiGetAuth(apiUrl.danhsachhochamhocvi, {
      ...param,
    });
  },
  danhSachDonViCongTac: (param) => {
    return apiGetAuth(apiUrl.danhsachdonvicongtac, {
      ...param,
    });
  },
  danhSachChucVu: (param) => {
    return apiGetAuth(apiUrl.danhsachchucvu, {
      ...param,
    });
  },
  BCThongKeKQNC: (param) => {
    return apiGetAuth(apiUrl.thongkekqnc, { ...param });
  },
  BCHoatDongNCKH: (param) => {
    return apiGetAuth(apiUrl.hoatdongnckh, { ...param });
  },
  BCTinhHinhVaKQThucHienNV: (param) => {
    return apiGetAuth(apiUrl.tinhhinhvakqthuchiennhiemvu, { ...param });
  },
  BaoCaoThongKeNhiemVuKhoaHoc: (param) => {
    return apiGetAuth(apiUrl.thongkenvkh, { ...param });
  },
  BaoCaoDanhSachNhiemVuKhoaHoc: (param) => {
    return apiGetAuth(apiUrl.danhsachnvkh, { ...param });
  },
  congBoKetQua: (param) => {
    return apiPostAuth(apiUrl.congboketqua, param);
  },
  ThongKeBCTheoChiSoISSN: (param) => {
    return apiGetAuth(apiUrl.thongkebctheochisoISSN, { ...param });
  },
};

export default api;
