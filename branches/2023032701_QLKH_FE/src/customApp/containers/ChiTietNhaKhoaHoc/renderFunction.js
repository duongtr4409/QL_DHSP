import React from "react";
import {Icon, Popover} from 'antd';
import moment from "moment";

export function renderQuaTrinhDaoTao(QuaTrinhDaoTao, editFunction, deleteFunction, disableEdit, disableDelete) {
  return <table className={'table-modal'}>
    <tr>
      <th style={{width: '20%'}}>Thời gian</th>
      <th style={{width: '25%'}}>Tên cơ sở đào tạo</th>
      <th style={{width: '25%'}}>Chuyên ngành</th>
      <th style={{width: '25%'}}>Học vị</th>
      <th style={{width: '5%'}}/>
    </tr>
    {QuaTrinhDaoTao.map((item, index) => (
      <tr>
        <td style={{textAlign: "center"}}>{item.KhoangThoiGian}</td>
        <td>{item.CoSoDaoTao}</td>
        <td>{item.ChuyenNganh}</td>
        <td>{item.HocVi}</td>
        <td style={{textAlign: "center"}}>
          <Icon type={'edit'} style={{marginRight: 10}} className={disableEdit ? 'not-allow' : 'pointer'}
                onClick={() => editFunction(index, 1)}/>
          <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                onClick={() => deleteFunction(index, 1)}/>
        </td>
      </tr>
    ))}
  </table>
}

export function renderQuaTrinhCongTac(QuaTrinhCongTac, editFunction, deleteFunction, disableEdit, disableDelete) {
  return <table className={'table-modal'}>
    <tr>
      <th style={{width: '20%'}}>Thời gian</th>
      <th style={{width: '25%'}}>Cơ quan công tác</th>
      <th style={{width: '25%'}}>Địa chỉ và điện thoại</th>
      <th style={{width: '25%'}}>Chức vụ</th>
      <th style={{width: '5%'}}/>
    </tr>
    {QuaTrinhCongTac.map((item, index) => {
      return <tr>
        <td style={{textAlign: 'center'}}>{item.KhoangThoiGian}</td>
        <td>{item.CoQuanCongTac}</td>
        <td>{item.DiaChiDienThoai}</td>
        <td>{item.ChucVu}</td>
        <td style={{textAlign: 'center'}}>
          <Icon type={'edit'} style={{marginRight: 10}} className={disableEdit ? 'not-allow' : 'pointer'}
                onClick={() => editFunction(index, 2)}/>
          <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                onClick={() => deleteFunction(index, 2)}/>
        </td>
      </tr>
    })}
  </table>
}

export function renderNgoaiNgu(NgoaiNgu, editFunction, deleteFunction, disableEdit, disableDelete) {
  return <table className={'table-modal'}>
    <tr>
      <th style={{width: '20%'}}>Ngoại ngữ</th>
      <th style={{width: '25%'}}>Đọc</th>
      <th style={{width: '25%'}}>Viết</th>
      <th style={{width: '25%'}}>Nói</th>
      <th style={{width: '5%'}}/>
    </tr>
    {NgoaiNgu.map((item, index) => {
      return <tr>
        <td style={{textAlign: 'center'}}>{item.TenNgoaiNgu}</td>
        <td style={{textAlign: 'center'}}>{item.Doc}</td>
        <td style={{textAlign: 'center'}}>{item.Viet}</td>
        <td style={{textAlign: 'center'}}>{item.Noi}</td>
        <td style={{textAlign: 'center'}}>
          <Icon type={'edit'} style={{marginRight: 10}} className={disableEdit ? 'not-allow' : 'pointer'}
                onClick={() => editFunction(index, 3)}/>
          <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                onClick={() => deleteFunction(index, 3)}/>
        </td>
      </tr>
    })}
  </table>
}

export function renderVanBangChungChi(VanBangChungChi, editFunction, deleteFunction, disableEdit, disableDelete) {
  return <table style={{width: '100%'}}>
    {VanBangChungChi.map((item, index) => {
      return <tr style={{height: 35}}>
        <td style={{textAlign: 'center', width: '10%'}}>
          <Icon type={'edit'} style={{marginRight: 10}} className={disableEdit ? 'not-allow' : 'pointer'}
                onClick={() => editFunction(index, 4)}/>
          <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                onClick={() => deleteFunction(index, 4)}/>
        </td>
        <td style={{textAlign: 'center', width: '20%'}}>
          {item.NgayCap ? moment(item.NgayCap).format('DD/MM/YYYY') : ""}
        </td>
        <td>
          {[[item.TieuDe, item.TrinhDo !== "" ? `(${item.TrinhDo})` : ""].filter(item => item !== "").join(" "), item.NoiCap, item.SoHieu].filter(item => item !== "").join(", ")}
        </td>
      </tr>
    })}
  </table>
}

export function renderGiaiThuongKhoaHoc(GiaiThuongKhoaHoc, editFunction, deleteFunction, disableEdit, disableDelete) {
  return <table style={{width: '100%'}}>
    {GiaiThuongKhoaHoc.map((item, index) => {
      return <tr style={{height: 35}}>
        <td style={{textAlign: 'center', width: '10%'}}>
          <Icon type={'edit'} style={{marginRight: 10}} className={disableEdit ? 'not-allow' : 'pointer'}
                onClick={() => editFunction(index, 5)}/>
          <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                onClick={() => deleteFunction(index, 5)}/>
        </td>
        <td style={{textAlign: 'center', width: '20%'}}>
          {item.KhoangThoiGian}
        </td>
        <td>{item.TieuDe}</td>
      </tr>
    })}
  </table>
}

export function renderDuAnDeTai(DuAnDeTai, editFunction, deleteFunction, disableEdit, disableDelete) {
  return <table className={'table-modal'}>
    <tr>
      <th style={{width: '5%'}}>STT</th>
      <th style={{width: '20%'}}>Tên đề tài/ dự án</th>
      <th style={{width: '25%'}}>Cơ quan tài trợ kinh phí</th>
      <th style={{width: '20%'}}>Thời gian thực hiện</th>
      <th style={{width: '25%'}}>Vai trò tham gia</th>
      <th style={{width: '5%'}}/>
    </tr>
    {DuAnDeTai.map((item, index) => {
      return <tr>
        <td style={{textAlign: 'center'}}>{index + 1}</td>
        <td>{item.TenDuAn}</td>
        <td>{item.CoQuanTaiTro}</td>
        <td style={{textAlign: 'center'}}>{item.KhoangThoiGian}</td>
        <td>{item.VaiTroThamGia}</td>
        <td style={{textAlign: 'center'}}>
          {!item.Disable ?
            <Icon type={'edit'} style={{marginRight: 10}} className={disableEdit ? 'not-allow' : 'pointer'}
                  onClick={() => editFunction(index, 6)}/> : ""}
          {!item.Disable ? <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                                 onClick={() => deleteFunction(index, 6)}/> : ""}
        </td>
      </tr>
    })}
  </table>
}

export function renderBaiBaoTapChi(BaiBaoTapChi, editFunction, deleteFunction, disableEdit, disableDelete, DanhSachCanBo) {
  return <table style={{width: '100%'}}>
    {BaiBaoTapChi.map((item, index) => {
      return <tr style={{height: 35}}>
        <td style={{textAlign: 'center', width: '10%'}}>
          {!item.Disable ? [<Icon type={'edit'} style={{marginRight: 10}}
                                  className={disableEdit ? 'not-allow' : 'pointer'}
                                  onClick={() => editFunction(index, 7)}/>,
            <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                  onClick={() => deleteFunction(index, 7)}/>] : ""}
        </td>
        <td style={{textAlign: 'center', width: '20%'}}>
          {item.NamDangTai}
        </td>
        <td>
          {item.LinkBaiBao !== "" ? <a href={item.LinkBaiBao}
                                       target={"_blank"}>{renderContentBaiBao(item, DanhSachCanBo)}</a> : renderContentBaiBao(item, DanhSachCanBo)}
        </td>
      </tr>
    })}
  </table>
}

export function renderKetQuaNghienCuu(KetQuaNghienCuu, editFunction, deleteFunction, disableEdit, disableDelete, DanhSachCanBo) {
  return <table style={{width: '100%'}}>
    {KetQuaNghienCuu.map((item, index) => {
      return <tr style={{height: 35}}>
        <td style={{textAlign: 'center', width: '10%'}}>
          {!item.Disable ?
            <Icon type={'edit'} style={{marginRight: 10}} className={disableEdit ? 'not-allow' : 'pointer'}
                  onClick={() => editFunction(index, 8)}/> : ""}
          {!item.Disable ? <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                                 onClick={() => deleteFunction(index, 8)}/> : ""}
        </td>
        <td style={{textAlign: 'center', width: '20%'}}>
          {item.NamXuatBan}
        </td>
        <td>
          {renderContentKetQuaNghienCuu(item, DanhSachCanBo)}
        </td>
      </tr>
    })}
  </table>
}

export function renderKetQuaNghienCuu2(KetQuaNghienCuu, editFunction, deleteFunction, disableEdit, disableDelete, DanhSachCanBo) {
  return <table className={'table-modal'}>
    <tr>
      <th style={{width: '5%'}}>STT</th>
      <th style={{width: '25%'}}>Tên công trình/ văn bằng</th>
      <th style={{width: '20%'}}>Thời gian</th>
      <th style={{width: '25%'}}>Tác giả</th>
      <th style={{width: '20%'}}>Ghi chú</th>
      <th style={{width: '5%'}}/>
    </tr>
    {KetQuaNghienCuu.map((item, index) => (
      <tr>
        <td style={{textAlign: "center"}}>{index + 1}</td>
        <td>{item.TieuDe}</td>
        <td style={{textAlign: "center"}}>{item.NamXuatBan}</td>
        <td>{renderTenCanBo(item.ListTacGia, DanhSachCanBo)}</td>
        <td>{item.GhiChu}</td>
        <td style={{textAlign: "center"}}>
          {!item.Disable ?
            <Icon type={'edit'} style={{marginRight: 10}} className={disableEdit ? 'not-allow' : 'pointer'}
                  onClick={() => editFunction(index, 8)}/> : ""}
          {!item.Disable ? <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                                 onClick={() => deleteFunction(index, 8)}/> : ""}
        </td>
      </tr>
    ))}
  </table>
}

export function renderSachChuyenKhao(SachChuyenKhao, editFunction, deleteFunction, disableEdit, disableDelete, DanhSachCanBo) {
  return <table style={{width: '100%'}}>
    {SachChuyenKhao.map((item, index) => {
      return <tr style={{height: 35}}>
        <td style={{textAlign: 'center', width: '10%'}}>
          {!item.Disable ? [<Icon type={'edit'} style={{marginRight: 10}}
                                  className={disableEdit ? 'not-allow' : 'pointer'}
                                  onClick={() => editFunction(index, 9)}/>,
            <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                  onClick={() => deleteFunction(index, 9)}/>] : ""}
        </td>
        <td style={{textAlign: 'center', width: '20%'}}>
          {item.NamXuatBan}
        </td>
        <td>
          {renderContentSachChuyenKhao(item, DanhSachCanBo)}
        </td>
      </tr>
    })}
  </table>
}

export function renderCacMonGiangDay(CacMonGiangDay, editFunction, deleteFunction, disableEdit, disableDelete) {
  return <table style={{width: '100%'}}>
    {CacMonGiangDay.map((item, index) => {
      return <tr style={{height: 35}}>
        <td style={{textAlign: 'center', width: '10%'}}>
          <Icon type={'edit'} style={{marginRight: 10}} className={disableEdit ? 'not-allow' : 'pointer'}
                onClick={() => editFunction(index, 10)}/>
          <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                onClick={() => deleteFunction(index, 10)}/>
        </td>
        <td style={{cursor: item.FileDinhKem.length > 0 ? 'pointer' : 'unset'}}>
          {item.FileDinhKem.length > 0 ? <Popover content={renderContentDownload(item.FileDinhKem)}>
            {item.TieuDe}{item.DeCuong !== "" ? ` (${item.DeCuong}) ` : ""}
          </Popover> : <span>{item.TieuDe}{item.DeCuong !== "" ? ` (${item.DeCuong}) ` : ""}</span>}
        </td>
      </tr>
    })}
  </table>
}

export function renderHoatDongKhoaHoc(HoatDongKhoaHoc, editFunction, deleteFunction, disableEdit, disableDelete) {
  return <table className={'table-modal'}>
    <tr>
      <th style={{width: '5%'}}>STT</th>
      <th style={{width: '45%'}}>Hoạt động khoa học khác</th>
      <th style={{width: '30%'}}>File đính kèm</th>
      <th style={{width: '15%'}}>Public lên CV</th>
      <th style={{width: '5%'}}/>
    </tr>
    {HoatDongKhoaHoc.map((item, index) => {
      return <tr>
        <td style={{textAlign: 'center'}}>{index + 1}</td>
        <td>{item.HoatDongKhoaHoc}</td>
        <td style={{textAlign: "center"}}>
          {item.FileDinhKem.length > 0 ? <Popover content={renderContentDownload(item.FileDinhKem)}>
            <Icon type={'cloud-download'} style={{fontSize: 15, color: 'blue'}}/>
          </Popover> : ""}
        </td>
        <td style={{textAlign: 'center'}}>
          {item.PublicCV ? <Icon type={'check'}/> : ""}
        </td>
        <td style={{textAlign: 'center'}}>
          <Icon type={'edit'} style={{marginRight: 10}} className={disableEdit ? 'not-allow' : 'pointer'}
                onClick={() => editFunction(index, 11)}/>
          <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                onClick={() => deleteFunction(index, 11)}/>
        </td>
      </tr>
    })}
  </table>
}

export function renderSanPhamDaoTao(SanPhamDaoTao, editFunction, deleteFunction, disableEdit, disableDelete) {
  return <table style={{width: '100%'}}>
    {SanPhamDaoTao.map((item, index) => {
      return <tr style={{height: 35}}>
        <td style={{textAlign: 'center', width: '10%'}}>
          {!item.Disable ? [<Icon type={'edit'} style={{marginRight: 10}}
                                  className={disableEdit ? 'not-allow' : 'pointer'}
                                  onClick={() => editFunction(index, 13)}/>,
            <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                  onClick={() => deleteFunction(index, 13)}/>] : ""}
        </td>
        <td style={{textAlign: 'center', width: '20%'}}>
          {item.NamBaoVe}
        </td>
        <td>
          {renderContentSanPhamDaoTao(item)}
        </td>
      </tr>
    })}
  </table>
}

export function renderSanPhamDaoTao2(SanPhamDaoTao, editFunction, deleteFunction, disableEdit, disableDelete) {
  return <table className={'table-modal'}>
    <tr>
      <th rowSpan={2} style={{width: '5%'}}>STT</th>
      <th rowSpan={2} style={{width: '15%'}}>Họ tên</th>
      <th colSpan={3}>Bậc học</th>
      <th colSpan={2}>Trách nhiệm</th>
      <th rowSpan={2} style={{width: '15%'}}>Tên khóa luận/ luận văn/ luận án</th>
      <th rowSpan={2} style={{width: '10%'}}>Thời gian hướng dẫn</th>
      <th rowSpan={2} style={{width: '15%'}}>Cơ sở đào tạo</th>
      <th rowSpan={2} style={{width: '10%'}}>Năm bảo vệ</th>
      <th rowSpan={2} style={{width: '10%'}}/>
    </tr>
    <tr>
      <th style={{width: '5%'}}>Cử nhân</th>
      <th style={{width: '5%'}}>Thạc sỹ</th>
      <th style={{width: '5%'}}>Tiến sỹ</th>
      <th style={{width: '5%'}}>Chính</th>
      <th style={{width: '5%'}}>Phụ</th>
    </tr>
    {SanPhamDaoTao.map((item, index) => <tr>
      <td style={{textAlign: 'center'}}>{index + 1}</td>
      <td>{item.TenHocVien}</td>
      <td style={{textAlign: 'center'}}>{item.LoaiDaoTao === 1 ? <Icon type={'check'}/> : ""}</td>
      <td style={{textAlign: 'center'}}>{item.LoaiDaoTao === 2 ? <Icon type={'check'}/> : ""}</td>
      <td style={{textAlign: 'center'}}>{item.LoaiDaoTao === 3 ? <Icon type={'check'}/> : ""}</td>
      <td style={{textAlign: 'center'}}>{item.CapHoiThao === 1 ? <Icon type={'check'}/> : ""}</td>
      <td style={{textAlign: 'center'}}>{item.CapHoiThao === 2 ? <Icon type={'check'}/> : ""}</td>
      <td>{item.TenLuanVan}</td>
      <td>{item.KhoangThoiGian}</td>
      <td>{item.CoSoDaoTao}</td>
      <td style={{textAlign: 'center'}}>{item.NamBaoVe}</td>
      <td style={{textAlign: 'center'}}>
        {!item.Disable ? [<Icon type={'edit'} style={{marginRight: 10}}
                                className={disableEdit ? 'not-allow' : 'pointer'}
                                onClick={() => editFunction(index, 13)}/>,
          <Icon type={'delete'} className={disableDelete ? 'not-allow' : 'pointer'}
                onClick={() => deleteFunction(index, 13)}/>] : ""}
      </td>
    </tr>)}
  </table>
}

export function logoHNUEBase64() {
  let hostPath = window.location.host;
  if (hostPath.includes('local')) {
    hostPath = "http://192.168.100.46:7003";
  } else {
    hostPath = "http://" + hostPath;
  }
  const logoPath = "Logo/logoHNUE-lylich.png";
  return `${hostPath}/${logoPath}`;
}

function renderContentDownload(fileDinhKem) {
  return <div style={{maxHeight: 300, overflowY: "auto"}}>
    {fileDinhKem.map(item => (
      <div style={{padding: 5, margin: 10, borderBottom: 'solid 1px #d6d6d6'}}>
        <a href={item.FileUrl} target={"_blank"} className={'link-hover'}>{item.TenFileGoc}</a>
      </div>
    ))}
  </div>
}

function renderContentSanPhamDaoTao(item) {
  let TenLoaiDaoTao = "";
  if (item.LoaiDaoTao) {
    switch (item.LoaiDaoTao) {
      case 1:
        TenLoaiDaoTao = "Cử nhân";
        break;
      case 2:
        TenLoaiDaoTao = "Thạc sỹ";
        break;
      case 3:
        TenLoaiDaoTao = "Tiến sỹ";
        break;
      case 4:
        TenLoaiDaoTao = "Nghiên cứu sinh";
        break;
    }
  }
  return [TenLoaiDaoTao, item.TenHocVien, item.TenLuanVan, item.NguoiHuongDan].filter(item => item !== "").join(", ");
}

export function renderContentSachChuyenKhao(item, DanhSachCanBo) {
  const TenChuBien = getTenCanBo(item.ChuBienID, item.CoQuanChuBienID, DanhSachCanBo);
  let ListTenTacGia = [];
  if (item.ListTacGia && item.ListTacGia.length) {
    item.ListTacGia.forEach(tg => ListTenTacGia.push(getTenCanBo(tg.CanBoID, tg.CoQuanID, DanhSachCanBo)))
  }
  ListTenTacGia = ListTenTacGia.filter(item => item !== "");
  const ListTenTacGiaStr = ListTenTacGia.length > 0 ? ListTenTacGia.join(", ") : "";
  return [TenChuBien, ListTenTacGiaStr, item.TenTapChiSachHoiThao, item.NhaXuatBan, item.ISSN].filter(item => item !== "").join(", ");
}

export function renderContentKetQuaNghienCuu(item, DanhSachCanBo) {
  let ListTenTacGia = [];
  if (item.ListTacGia && item.ListTacGia.length) {
    item.ListTacGia.forEach(tg => ListTenTacGia.push(getTenCanBo(tg.CanBoID, tg.CoQuanID, DanhSachCanBo)))
  }
  ListTenTacGia = ListTenTacGia.filter(item => item !== "");
  const ListTenTacGiaStr = ListTenTacGia.length > 0 ? ListTenTacGia.join(", ") : "";
  return [ListTenTacGiaStr, item.TieuDe, item.GhiChu].filter(item => item !== "" && item != null).join(", ");
}

export function renderContentBaiBao(item, DanhSachCanBo) {
  let ListTenTacGia = [];
  if (item.ListTacGia && item.ListTacGia.length) {
    item.ListTacGia.forEach(tg => ListTenTacGia.push(getTenCanBo(tg.CanBoID, tg.CoQuanID, DanhSachCanBo)))
  }
  ListTenTacGia = ListTenTacGia.filter(item => item !== "");
  const ListTenTacGiaStr = ListTenTacGia.length > 0 ? ListTenTacGia.join(", ") : "";
  if (item.LoaiBaiBao === 1) {
    let subContent = `(${[item.HeSoAnhHuong, getTenChiSo(item.ChiSo), getRankName(item.RankSCIMAG)].filter(item => item !== "").join(", ")})`;
    subContent = subContent === "()" ? "" : subContent;
    const soISSN = item.ISSN !== "" ? `ISSN: ${item.ISSN}` : "";
    return [ListTenTacGiaStr, item.TieuDe, [item.TenTapChiSachHoiThao, subContent].filter(item => item !== "").join(" "), soISSN, item.Tap, item.So, item.Trang, item.NhaXuatBan].filter(item => item !== "").join(", ");
  } else if (item.LoaiBaiBao === 2) {
    let subContent = `(${[getDiemTapChi(item.DiemTapChi)].filter(item => item !== "").join(", ")})`;
    subContent = subContent === "()" ? "" : subContent;
    const soISSN = item.ISSN !== "" ? `ISSN: ${item.ISSN}` : "";
    return [ListTenTacGiaStr, item.TieuDe, [item.TenTapChiSachHoiThao, subContent].filter(item => item !== "").join(" "), soISSN, item.Tap, item.So, item.Trang, item.NhaXuatBan].filter(item => item !== "").join(", ");
  } else if (item.LoaiBaiBao === 3 || item.LoaiBaiBao === 4) {
    let subContent = `(${[getTenCapHoiThao(item.CapHoiThao)].filter(item => item !== "").join(", ")})`;
    subContent = subContent === "()" ? "" : subContent;
    const soISSN = item.ISSN !== "" ? `ISBN: ${item.ISSN}` : "";
    const NgayHoiThao = item.NgayHoiThao ? moment(item.NgayHoiThao).format("DD/MM/YYYY") : "";
    return [ListTenTacGiaStr, item.TieuDe, [item.TenTapChiSachHoiThao, subContent].filter(item => item !== "").join(" "), soISSN, item.Tap, item.So, item.Trang, item.NhaXuatBan, NgayHoiThao, item.DiaDiemToChuc].filter(item => item !== "").join(", ");
  }
}

function getRankName(rank) {
  return rank ? `Q${rank}` : ""
}

function getTenChiSo(chiso) {
  let name = "";
  if (chiso) {
    name = chiso === 1 ? "ISI" : chiso === 2 ? "SCOPUS" : "Khác";
  }
  return name;
}

function getTenCapHoiThao(cap) {
  if (cap) {
    return cap === 1 ? "Quốc tế" : cap === 2 ? "Quốc gia" : "Trong nước";
  }
  return "";
}

function getDiemTapChi(diem) {
  if (diem) {
    if (diem === 1) {
      return 0
    } else if (diem === 2) {
      return 0.25
    } else if (diem === 3) {
      return 0.5
    } else if (diem === 4) {
      return 0.75
    } else if (diem === 5) {
      return 1
    }
  }
  return "";
}

function getTenCanBo(CanBoID, CoQuanID, DanhSachCanBo) {
  const canbo = DanhSachCanBo.filter(item => item.CanBoID == CanBoID && item.CoQuanID == CoQuanID)[0];
  return canbo ? canbo.TenCanBo : "";
}

function renderTenCanBo(ListID, DanhSachCanBo) {
  let ListTenTacGia = [];
  if (ListID && ListID.length) {
    ListID.forEach(tg => ListTenTacGia.push(getTenCanBo(tg.CanBoID, tg.CoQuanID, DanhSachCanBo)))
  }
  ListTenTacGia = ListTenTacGia.filter(item => item !== "");
  return ListTenTacGia.length > 0 ? ListTenTacGia.join(", ") : "";
}





