/**
 * 转化成中文小写字符串
 *
 */
export function simplifiedOrdinary(value: number): string {
  const cnNums = ['零', '一', '二', '三', '四', '五', '六', '七', '八', '九']; // 数字
  const cnIntRadice = ['', '十', '百', '千']; // 基本单位
  const cnIntUnits = ['', '万', '亿', '兆']; // 扩展单位

  if (value === 0) return '零';

  let integerNum = Math.floor(value).toString();
  let chineseStr = '';

  let unitPos = 0; // 节权位
  let zeroCount = 0;

  while (integerNum.length > 0) {
    let section = integerNum.length > 4 ? integerNum.slice(-4) : integerNum;
    integerNum = integerNum.length > 4 ? integerNum.slice(0, -4) : '';

    let sectionStr = '';
    let zero = true;

    for (let i = 0; i < section.length; i++) {
      const digit = Number(section[section.length - 1 - i]);
      if (digit === 0) {
        if (!zero) {
          zero = true;
          sectionStr = cnNums[0] + sectionStr;
        }
      } else {
        zero = false;
        sectionStr = cnNums[digit] + cnIntRadice[i] + sectionStr;
      }
    }

    sectionStr = sectionStr.replace(/零+$/g, ''); // 去尾零
    if (sectionStr !== '') {
      sectionStr += cnIntUnits[unitPos];
    }
    chineseStr = sectionStr + chineseStr;
    unitPos++;
  }

  // 处理连续零
  chineseStr = chineseStr.replace(/零+/g, '零');
  // “一十”开头时可省略“一”
  chineseStr = chineseStr.replace(/^一十/, '十');

  return chineseStr;
}
