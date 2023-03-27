import styled from 'styled-components';
import { palette } from 'styled-theme';

const BoxWrapper = styled.div`
  width: calc(100% - 45px);
  height: 100%;
  padding: 20px;
  background-color: #fff;
  border: 1px solid ${palette('border', 0)};
  border-radius: 10px;
  // border: 1px solid #999;
  margin: 5px 0 30px 20px;
 
}

  &:last-child {
    margin-bottom: 0;
  }

  @media only screen and (max-width: 767px) {
    padding: 20px;
    ${'' /* margin: 0 10px 30px; */};
  }

  &.half {
    width: calc(50% - 34px);
    @media (max-width: 767px) {
      width: 100%;
    }
  }
`;

export { BoxWrapper };
