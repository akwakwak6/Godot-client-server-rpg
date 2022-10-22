using System;
using System.Threading.Tasks;
using System.Threading;
public class Delay {

    private CancellationTokenSource  src;

    public async Task wait(int ms){
        try{
            src = new CancellationTokenSource();
            await Task.Delay(ms,src.Token);
        }catch(Exception e){

        }
    }

    public void Cancel(){
        src?.Cancel();
    }

}