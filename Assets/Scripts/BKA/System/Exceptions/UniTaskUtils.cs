using System;
using Cysharp.Threading.Tasks;

namespace BKA.System.Exceptions
{
    public static class UniTaskUtils
    {
        public static async UniTask WithPostCancellation(this UniTask uniTask, Action action)
        {
            try
            {
                await uniTask;
            }
            catch (OperationCanceledException e)
            {
                action();
                throw;
            }
        }
        
        public static async UniTask<T> WithPostCancellation<T>(this UniTask<T> uniTask, Action action)
        {
            try
            {
                var temp = await uniTask;
                return temp;
            }
            catch (OperationCanceledException e)
            {
                action();
                throw;
            }
        }
    }
}